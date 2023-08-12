using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using BlogAPI.PostgreSQL;
using BlogAPI.Models;
using Microsoft.AspNetCore.Authorization;
using BlogAPI.Services;
using StackExchange.Redis;
using System.Data.Entity;
using BlogAPI.Repositories;

namespace BlogAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private readonly BlogContext _db;
    private readonly IConfiguration _configuration;
    private readonly ICredentialsService _credentialsService;
    private readonly IAccountService _accountService;

    private readonly IUserRepository _userRepository;

    public UserController(BlogContext context, IConfiguration config, ICredentialsService credentialsService, IAccountService accountService, IUserRepository userRepository) {
        _db = context;
        _configuration = config;
        _credentialsService = credentialsService;
        _accountService = accountService;
        _userRepository = userRepository;
    }

    [HttpGet("{id}"), Authorize]
    public async Task<ActionResult<UserDto>> GetUserAsync(int id)
    {
        // checks jwt token credentials match request id (user making request for id is the owner)
        if (await _accountService.ResolveUser(id)) {
            var user = await _userRepository.GetUser(id);
            return user == null ? BadRequest("User does not exist") : Ok(user.AsDto());
        }
        return Unauthorized("Access denied");
    }

    [HttpPut("{id}"), Authorize]
    public async Task<ActionResult<UserDto>> UpdateUserAsync(int id, UserDto request)
    {
        if (await _accountService.ResolveUser(id))
        {
            var updatedUser = await _userRepository.UpdateUser(id, request);
            return updatedUser == null ? BadRequest("User doesn't exits") : Ok(updatedUser.AsDto().Stringify());
        }
        return Unauthorized("Access denied");
    }

    [HttpDelete("{id}"), Authorize]
    public async Task<IActionResult> RemoveUserAsync(int id)
    {
        // checks jwt token credentials match request id (user making request for id is the owner)
        if (await _accountService.ResolveUser(id))
        {
            _userRepository.DeleteUser(id);
            _accountService.Blacklist();
        }
        return Unauthorized("Access denied");
    }

    [HttpPost, AllowAnonymous]
    public async Task<ActionResult<UserDto>> CreateUserAsync(Credentials request)
    {
        if (_userRepository.EmailExists(request.email))
        {
            return BadRequest("Email address is taken, please try another.");
        }

        _credentialsService.CreatePasswordHash(request.password, out byte[] passwordHash, out byte[] passwordSalt);

        var newUser = new UserEntity
        {
            EmailAddress = request.email,
            UserName = request.username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
        };
        _userRepository.AddUser(newUser);
   
        return Ok(newUser.AsDto());
    }

    [HttpPost("auth"), AllowAnonymous]
    public async Task<ActionResult<string>> AuthenticateAsync(Credentials request)
    {
        var user = _db.Users.FirstOrDefault(user => user.EmailAddress == request.email && user.UserName == request.username);
        if (user == null || !_credentialsService.VerifyPasswordHash(request.password, user.PasswordHash, user.PasswordSalt))
        {
            return BadRequest("Invalid credentials");
        }
    
        var token = _credentialsService.CreateToken(user);

        return Ok(token);
    }

    [HttpGet("/User/{id}/Post"), Authorize]
    public async Task<IActionResult> GetAllPostsForIdAsync(int id)
    {
        var author = await _db.Users.FindAsync(id);
        var posts =
            await _accountService.ResolveUser(id) ?
             _db.Posts.Where(p => p.Author == author).Select(p => p.AsDto())
            :
             _db.Posts.Where(p => p.Author == author && !p.IsPrivate).Select(p => p.AsDto());
        return Ok(posts.AsEnumerable().ToList());
    }

    // TODO: Delete this method
    [HttpPost("genUsers"), AllowAnonymous]
    public async Task<ActionResult> GenerateUsers()
    {
        var names = new string[] { "akshay", "vai", "savita" };
        var emails = new string[] { "akshay@gmail.com", "vai@gmail.com", "savita@gmail.com" };
        var passwords = names.Select(n => n).ToArray();

        for (int i=0;i<=2;i++)
        {
            await CreateUserAsync(new Credentials { email = emails[i], username = names[i], password = passwords[i] });
        }
        return Ok();
    }
}