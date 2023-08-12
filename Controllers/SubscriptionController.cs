using BlogAPI.PostgreSQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriptionController : Controller
{
    private readonly BlogContext _db;
    private readonly IConfiguration _configuration;
    public SubscriptionController(BlogContext context, IConfiguration config) {
        _db = context;
        _configuration = config;
    }

    // [HttpGet("GetFollowing"), Authorize]
    // public IActionResult GetFollowing()
    // {
    //     var subs = _db.UserIdentities.Select(u => u.FollowingUser).ToList();
    //     return Ok(subs);
    // }


    [HttpGet("GetFollowing"), Authorize]
    public async Task<ActionResult<List<string>>> GetFollowing()
    {
        var thisUser = await _db.Users.FirstOrDefaultAsync(user => user.EmailAddress == User.FindFirstValue(ClaimTypes.Email));
        var followers = await _db.Users.Where(user => user.Following.Contains(thisUser)).Select(u => u.UserName).ToListAsync();
        
        return Ok(followers);
    }
    
    [HttpGet("GetFollowers"), Authorize]
    public async Task<ActionResult<List<string>>> GetFollowers()
    {
        var thisUser = await _db.Users.FirstOrDefaultAsync(user => user.EmailAddress == User.FindFirstValue(ClaimTypes.Email));
        var following = thisUser.Following.Select(user => user.UserName).ToList();

        return Ok(following);
    }

    [HttpPost("Follow"), Authorize]
    public async Task<IActionResult> Follow(int toId)
    {
        var userToFollow = await _db.Users.FindAsync(toId);
        if (userToFollow == null)
        {
            return BadRequest("User not found!");
        }

        var thisUser = await _db.Users.FirstOrDefaultAsync(u => u.EmailAddress == User.FindFirstValue(ClaimTypes.Email));

        if (thisUser.Following == null)
        {
            thisUser.Following = new List<User>() { userToFollow };
        }
        else
        {
            thisUser.Following.Add(userToFollow);
        }
        await _db.SaveChangesAsync();

        return Ok(thisUser.Following.Select(u => u.UserName));
    }

    [HttpPut("Unfollow"), Authorize]
    public async Task<IActionResult> Unfollow(int userToUnfollowId)
    {
        var thisUser = _db.Users.FirstOrDefault(u => u.EmailAddress == User.FindFirstValue(ClaimTypes.Email));
        var userToUnfollow = await _db.Users.FindAsync(userToUnfollowId);
        
        // TODO: FOLLOW CHECK NOT WORKING CORRECTLY
        if (!thisUser.Following.Contains(userToUnfollow))
        {
            return BadRequest("User not followed");
        }

        thisUser.Following.Remove(userToUnfollow);
    
        await _db.SaveChangesAsync();
    
        return Ok(thisUser.Following);
    }
}