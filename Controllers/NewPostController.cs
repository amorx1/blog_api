using Microsoft.AspNetCore.Mvc;
using BlogAPI.Models;
using BlogAPI.Dtos;
using BlogAPI.Interfaces;
using BlogAPI.Repositories;
using AutoMapper;

namespace BlogAPI.Controllers
{
	[ApiController]
	[Route("api")]
	public class PostsController : BaseController<PostEntity, PostReadDto, PostWriteDto, EFPostRepository>
	{
		private readonly EFPostRepository _postRepository;
		private readonly EFUserRepository _userRepository;
		private readonly IMapper _mapper;
		private readonly IAccountService _accountService;
		
		public PostsController(EFPostRepository postRepository, EFUserRepository userRepository, IAccountService accountService, IMapper mapper) : base(postRepository, mapper)
		{
			_postRepository  = postRepository;
			_userRepository = userRepository;
			_mapper = mapper;
			_accountService = accountService;
		}

		[HttpGet("user/{userId}/posts")]
		public async Task<ActionResult<ICollection<PostReadDto?>>> GetAllAsync([FromRoute] int userId)
		{
			var isOwner = _accountService.ResolveUser(userId); // task
			var posts = await  _postRepository.GetAllAsync(userId); // task

			if (await isOwner)
			{
				return Ok(posts.Select(p => _mapper.Map<PostReadDto>(p)));
			}
			else
			{
				// request is not by owner -> omit private posts
				return Ok(posts.Where(p => !p.IsPrivate).Select(p => _mapper.Map<PostReadDto>(p)));
			}
		}

		[HttpGet("user/{userId}/posts/{postId}")]
		public override async Task<ActionResult<PostReadDto?>> GetAsync([FromRoute] int userId, [FromRoute] int postId)
		{
			var isOwner = _accountService.ResolveUser(userId);
			var post = await _postRepository.GetAsync(postId);
						
			if (post is null)
			{
				return NotFound();
			}
			
			if (!await isOwner && post.IsPrivate)
			{
				return Unauthorized();
			}
			else
			{
				return _mapper.Map<PostReadDto>(post);
			}
			
		}
		
		[HttpDelete("user/{userId}/posts/{postId}")]
		public override async Task<ActionResult<PostReadDto?>> RemoveAsync([FromRoute] int userId, [FromRoute] int postId)
		{
			var isOwner = await _accountService.ResolveUser(userId);
			if (!isOwner)
			{
				return Unauthorized();
			}
			else
			{
				var deletedPost = await _postRepository.DeleteAsync(postId);
				if (deletedPost is null)
				{
					return NotFound();
				}
				return Ok(_mapper.Map<PostReadDto>(deletedPost));
			}
		}

		[HttpPut("user/{userId}/posts/{postId}")]
		public override async Task<ActionResult<PostReadDto?>> UpdateAsync([FromRoute] int userId, [FromRoute] int postId, [FromBody] PostWriteDto request)
		{
			var isOwner = await _accountService.ResolveUser(userId);
			if (!isOwner)
			{
				return Unauthorized();
			}
			else
			{
				var postToUpdate = await _postRepository.GetAsync(postId);
				if (postToUpdate is null)
				{
					return NotFound();
				}

				postToUpdate.Title = request.Title;
				postToUpdate.Body = request.Body;
				postToUpdate.IsPrivate = request.IsPrivate;
				
				var updatedPost = await _postRepository.UpdateAsync(postToUpdate);

				return Ok(_mapper.Map<PostReadDto>(updatedPost));
			}
		}

		[HttpPost("user/{userId}/posts/new")]
		public override async Task<ActionResult<PostReadDto?>> CreateAsync(int userId, [FromBody] PostWriteDto request)
		{
			var isOwner = await _accountService.ResolveUser(userId);
			var user = await _userRepository.GetAsync(userId);
			
			if (!isOwner || userId != request.AuthorId)
			{
				return Unauthorized();
			}
			
			var newPost = _mapper.Map<PostEntity>(request, opt => 
				opt.AfterMap((source, destination) =>
					{
						destination.Author = user;
					}
				));
			
			var createdPost = await _postRepository.AddAsync(newPost);
			return Ok(_mapper.Map<PostReadDto>(createdPost));
		}
	}
}