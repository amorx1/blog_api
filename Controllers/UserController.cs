using Microsoft.AspNetCore.Mvc;
using BlogAPI.PostgreSQL;
using BlogAPI.Models;
using Microsoft.AspNetCore.Authorization;
using BlogAPI.Interfaces;

namespace BlogAPI.Controllers;

[ApiController]
[Route("api")]
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

    [HttpGet("user/{id}"), Authorize]
    public async Task<ActionResult<UserDto>> GetUserAsync(int id)
    {
        // checks jwt token credentials match request id (user making request for id is the owner)
        if (await _accountService.ResolveUser(id)) {
            var user = await _userRepository.GetUser(id);
            return user is null ? BadRequest("User does not exist") : Ok(user.AsDto());
        }
        return Unauthorized("Access denied");
    }

    [HttpPut("user/{id}"), Authorize]
    public async Task<ActionResult<UserDto>> UpdateUserAsync(int id, UserDto request)
    {
        if (await _accountService.ResolveUser(id))
        {
            var updatedUser = await _userRepository.UpdateUser(id, request);
            return updatedUser is null ? BadRequest("User doesn't exit") : Ok(updatedUser.AsDto().Stringify());
        }
        return Unauthorized("Access denied");
    }

    [HttpDelete("user/{id}"), Authorize]
    public async Task<ActionResult> RemoveUserAsync(int id)
    {
        // checks jwt token credentials match request id (user making request for id is the owner)
        if (await _accountService.ResolveUser(id))
        {
            var success = await _userRepository.DeleteUser(id);
            if (success)
            {
                _accountService.Blacklist();
                return Ok("Deleted");
            }
            return BadRequest("User not found");
        }
        return Unauthorized("Access denied");
    }
    
    [HttpPost("user/new"), AllowAnonymous]
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

        var success = await _userRepository.AddUser(newUser);
        return success ? Ok(newUser.AsDto()) : Problem("Error creating user");
       }

    [HttpPost("authenticate"), AllowAnonymous]
    public async Task<ActionResult<string>> AuthenticateAsync(Credentials request)
    {
        var user = _db.Users.FirstOrDefault(user => user.EmailAddress == request.email && user.UserName == request.username);
        if (user is null || !_credentialsService.VerifyPasswordHash(request.password, user.PasswordHash, user.PasswordSalt))
        {
            return BadRequest("Invalid credentials");
        }
    
        var token = _credentialsService.CreateToken(user);

        return Ok(token);
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