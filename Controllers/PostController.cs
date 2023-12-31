using Microsoft.AspNetCore.Mvc;
using BlogAPI.Models;
using BlogAPI.Dtos;
using BlogAPI.Interfaces;
using BlogAPI.Repositories;
using BlogAPI.AttributeFiters;
using AutoMapper;

namespace BlogAPI.Controllers
{
	[ApiController]
	[Route("api")]
	public class PostsController : BaseController<PostEntity, PostReadDto, PostWriteDto, EFPostRepository>
	{
		private readonly EFPostRepository _postRepository;
		private readonly EFUserRepository _userRepository;
		private readonly EFImageRepository _imageRepository;
		private readonly IMapper _mapper;
		private readonly IAccountService _accountService;
		
		public PostsController(EFPostRepository postRepository, EFUserRepository userRepository, EFImageRepository imageRepository, IAccountService accountService, IMapper mapper) : base(postRepository, mapper)
		{
			_postRepository  = postRepository;
			_userRepository = userRepository;
			_imageRepository = imageRepository;
			_mapper = mapper;
			_accountService = accountService;
		}

		[HttpGet("user/{userId}/posts"), OwnerVerificationFilter]
		public async Task<ActionResult<ICollection<PostReadDto?>>> GetAllAsync([FromRoute] int userId, bool isOwner)
		{
			var posts = await  _postRepository.GetAllAsync(userId);

			if (isOwner)
			{
				return Ok(posts.Select(p => _mapper.Map<PostReadDto>(p)));
			}
			else
			{
				// request is not by owner -> omit private posts
				return Ok(posts.Where(p => !p.IsPrivate).Select(p => _mapper.Map<PostReadDto>(p)));
			}
		}

		[HttpGet("user/{userId}/posts/{postId}"), OwnerVerificationFilter]
		public override async Task<ActionResult<PostReadDto?>> GetAsync([FromRoute] int userId, [FromRoute] int postId, bool isOwner)
		{

			var post = await _postRepository.GetAsync(postId);
			if (post is null)
			{
				return NotFound();
			}

			if (post.IsPrivate && !isOwner)
			{
				return Unauthorized();
			}
			
			return _mapper.Map<PostReadDto>(post);
					
		}
		
		[HttpDelete("user/{userId}/posts/{postId}")]
		public override async Task<ActionResult<PostReadDto?>> RemoveAsync([FromRoute] int userId, [FromRoute] int postId, bool isOwner)
		{
			if (!isOwner)
			{
				return Unauthorized();
			}

			var deletedPost = await _postRepository.DeleteAsync(postId);
			if (deletedPost is null)
			{
				return NotFound();
			}
			return Ok(_mapper.Map<PostReadDto>(deletedPost));
		}

		[HttpPut("user/{userId}/posts/{postId}")]
		public override async Task<ActionResult<PostReadDto?>> UpdateAsync([FromRoute] int userId, [FromRoute] int postId, [FromBody] PostWriteDto request, bool isOwner)
		{
			if (!isOwner || userId != request.AuthorId)
			{
				return Unauthorized();
			}

			var postToUpdate = await _postRepository.GetAsync(postId);
			if (postToUpdate is null)
			{
				return NotFound();
			}
			
			if (request.Title is not null)
			{
				postToUpdate.Title = request.Title;
			}
			if (request.Body is not null)
			{
				postToUpdate.Body = request.Body;
			}
			if (request.Images is not null)
			{
				postToUpdate.Images = request.Images.Select(i => _mapper.Map<ImageEntity>(i)).ToList();
			}
			postToUpdate.IsPrivate = request.IsPrivate;				
			
			var updatedPost = await _postRepository.UpdateAsync(postToUpdate);
			return Ok(_mapper.Map<PostReadDto>(updatedPost));
		}

		[HttpPost("user/{userId}/posts/new")]
		public override async Task<ActionResult<PostReadDto?>> CreateAsync(int userId, [FromBody] PostWriteDto request, bool isOwner)
		{
			if (!isOwner || userId != request.AuthorId)
			{
				return Unauthorized();
			}

			var newPost = _mapper.Map<PostEntity>(request);
			var createdPost = await _postRepository.AddAsync(newPost);
			
			return Ok(_mapper.Map<PostReadDto>(createdPost));
		}
	}
}