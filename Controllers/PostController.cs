using BlogAPI.Models;
using BlogAPI.PostgreSQL;
using BlogAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly BlogContext _db;
    private readonly IConfiguration _configuration;
    private readonly IAccountService _accountService;

    public PostController(BlogContext context, IConfiguration config, IAccountService accountService)
    {
        _db = context;
        _configuration = config;
        _accountService = accountService;
    }

    [HttpPut("{postId}"), Authorize]
    public async Task<IActionResult> UpdatePostAsync(int postId, PostDto request)
    {
        if (await _accountService.ResolveUser(request.AuthorId))
        {
            var author = await _db.Users.FindAsync(_accountService.GetUserId());
            var post = await _db.Posts.FirstOrDefaultAsync(p => p.Author == author && p.Id == postId);
            post.Update(request);

            _db.Posts.Update(post);
            await _db.SaveChangesAsync();

            return Ok(post.AsDto());
        }
        return BadRequest("Access denied");
    }

    [HttpDelete("{postId}"), Authorize]
    public async Task<IActionResult> DeletePostAsync(int postId, PostDto request)
    {
        if (await _accountService.ResolveUser(request.AuthorId))
        {
            var author = await _db.Users.FindAsync(_accountService.GetUserId());
            var post = await _db.Posts.FirstOrDefaultAsync(p => p.Author == author && p.Id == postId);

            _db.Posts.Remove(post);
            await _db.SaveChangesAsync();

            return Ok($"Deleted post with Id: {postId}");
        }
        return BadRequest("Access denied");
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> CreatePostAsync(PostDto request)
    {
        if (await _accountService.ResolveUser(request.AuthorId))
        {
            var author = await _db.Users.FindAsync(_accountService.GetUserId());
            if (author == null)
            {
                return BadRequest("Author does not exist");
            }
            var newPost = new PostEntity
            {
                Title = request.Title,
                Body = request.Body,
                Author = author,
                IsPrivate = request.IsPrivate
            };

            _db.Posts.Add(newPost);
            await _db.SaveChangesAsync();

            return Ok(newPost.AsDto());
        }
        return BadRequest("Access denied");
    }

    //[HttpDelete("DeletePost"), Authorize]
    //public async Task<IActionResult> DeletePost(int postToBeDeletedId)
    //{
    //    var thisUser = await _db.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
    //    var postToBeDeleted = _db.Posts.Where(p => p.UserId == thisUser.UserId).FirstOrDefault(p => p.PostId == postToBeDeletedId);

    //    if (postToBeDeleted == null)
    //    {
    //        return BadRequest("The post either does not exist or you do not have authorization to delete it");
    //    }

    //    _db.Posts.Remove(postToBeDeleted);
    //    await _db.SaveChangesAsync();
    //    return Ok("Deleted post!");
    //}
}