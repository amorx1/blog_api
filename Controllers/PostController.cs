using BlogAPI.Models;
using BlogAPI.PostgreSQL;
using BlogAPI.Repositories;
using BlogAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Controllers;

[ApiController]
[Route("Posts")]
public class PostController : ControllerBase
{
    private readonly BlogContext _db;
    private readonly IConfiguration _configuration;
    private readonly IAccountService _accountService;
    private readonly IPostRepository _postRepository;

    public PostController(BlogContext context, IConfiguration config, IAccountService accountService, IPostRepository postRepository)
    {
        _db = context;
        _configuration = config;
        _accountService = accountService;
        _postRepository = postRepository;
    }

    [HttpPut("{postId}"), Authorize]
    public async Task<ActionResult> UpdatePostAsync(int postId, PostDto request)
    {
        if (await _accountService.ResolveUser(request.AuthorId))
        {
	        return Ok(await _postRepository.UpdatePost(postId, request));
        }
        return Unauthorized("Access denied");
    }

    [HttpDelete("{postId}"), Authorize]
    public async Task<ActionResult> DeletePostAsync(int postId, int authorId)
    {
        if (await _accountService.ResolveUser(authorId))
        {
            var success = await _postRepository.DeletePost(postId, authorId);
            return success ? Ok($"Deleted post ID: {postId} by user ID: {authorId}") : Problem("Error deleting post");
        }
        return Unauthorized("Access denied");
    }

    [HttpPost, Authorize]
    public async Task<ActionResult> CreatePostAsync(PostDto request)
    {
        if (await _accountService.ResolveUser(request.AuthorId))
        {
            var newPost = await _postRepository.AddPost(request);
            return (newPost == null) ? Problem("Error creating post") : Ok(newPost);
        }
        return Unauthorized("Access denied");
    }

    [HttpGet("{postId}"), Authorize]
    public async Task<ActionResult<PostDto>> GetPostById(int postId)
    {
        var post = await  _postRepository.GetPost(postId);
        if (post == null)
        {
            return BadRequest("Post does not exist");
        }
        if (!post.IsPrivate || (post.IsPrivate && await _accountService.ResolveUser(post.AuthorId)))
        {
            return Ok(post);
        }
        return Unauthorized("Access denied");
        
    }

    [HttpGet, Authorize]
    public async Task<ActionResult> GetPostsForUser(int userId)
    {
        var author = await _db.Users.FindAsync(userId);
        var posts =
            await _accountService.ResolveUser(userId) ?
             _db.Posts.Where(p => p.Author == author).Select(p => p.AsDto())
            :
             _db.Posts.Where(p => p.Author == author && !p.IsPrivate).Select(p => p.AsDto());
        return Ok(posts.AsEnumerable().ToList());
    }
}