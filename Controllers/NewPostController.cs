using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlogAPI.Models;
using BlogAPI.Dtos;
using BlogAPI.Controllers;
using BlogAPI.Services;
using BlogAPI.Interfaces;
using BlogAPI.Repositories;
using AutoMapper;

// TODO: PostReadDto incorrectly mapping AuthorId to always 0

namespace BlogAPI.Controllers
{
	[ApiController]
	[Route("api")]
	public class PostsController : BaseController<PostEntity, PostReadDto, PostWriteDto, EFPostRepository>
	{
		private readonly EFPostRepository _repository;
		private readonly IMapper _mapper;
		private readonly IAccountService _accountService;
		
		public PostsController(EFPostRepository repository, IAccountService accountService, IMapper mapper) : base(repository, mapper)
		{
			_repository  = repository;
			_mapper = mapper;
			_accountService = accountService;
		}

		// Get all posts for a specific user -> if authenticated then include private posts
		[HttpGet("user/{userId}/posts")]
		public async Task<ActionResult<ICollection<PostReadDto?>>> GetAllAsync([FromRoute] int userId)
		{
			var isOwner = _accountService.ResolveUser(userId); // task
			var posts = await  _repository.GetAllAsync(userId); // task

			if (await isOwner)
			{
				return Ok(posts.Select(p => _mapper.Map<PostReadDto>(p)));
			}
			else
			{
				return Ok(posts.Where(p => !p.IsPrivate).Select(p => _mapper.Map<PostReadDto>(p)));
			}
		}

		[HttpGet("user/{userId}/posts/{postId}")]
		public override async Task<ActionResult<PostReadDto?>> GetAsync([FromRoute] int userId, [FromRoute] int postId)
		{
			var isOwner = _accountService.ResolveUser(userId);
			var post = await _repository.GetAsync(postId);
						
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
				var response = _mapper.Map<PostReadDto>(post);
				// Console.WriteLine(response.AuthorId);
				return response;
			}
			
		}

		public override async Task<ActionResult<PostReadDto?>> RemoveAsync([FromRoute] int id)
		{
			throw new NotImplementedException();
		}

		public override async Task<ActionResult<PostReadDto?>> UpdateAsync([FromRoute] int id, PostWriteDto request)
		{
			throw new NotImplementedException();
		}

		public override async Task<ActionResult<PostReadDto?>> CreateAsync([FromBody] PostWriteDto request)
		{
			throw new NotImplementedException();
		}
	}
}
