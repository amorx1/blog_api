using System;
using BlogAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Models;
using BlogAPI.Repositories;
using BlogAPI.PostgreSQL;

namespace BlogAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class NewUserController : BaseController<UserEntity, UserDto, Credentials, EFUserRepository>
    {
        private readonly EFUserRepository repository;
        private readonly ICredentialsService credentialsService;
        private readonly IAccountService accountService;

        public NewUserController(EFUserRepository repository, IAccountService accountService, ICredentialsService credentialsService)
        : base(repository, accountService, credentialsService)
        {
            this.repository = repository;
            this.credentialsService = credentialsService;
            this.accountService = accountService;
        }
        public override UserDto? DTO(UserEntity entity)
        {
            return (entity is null) ? null : new UserDto
            {
                Id = entity.Id,
                UserName = entity.UserName,
                EmailAddress = entity.EmailAddress,
            };
        }

        [HttpGet("newuser/{id}"), Authorize, AuthenticationFilter]
        public override async Task<ActionResult<UserDto?>> GetAsync([FromRoute] int id)
        {
            var entity = await repository.GetAsync(id);
            return entity is null ? BadRequest("Does not exist!") : Ok(DTO(entity));
        }

        [HttpDelete("newuser/{id}"), Authorize, AuthenticationFilter]
        public override async Task<ActionResult<UserDto?>> RemoveAsync([FromRoute] int id)
        {
            var deletedEntity = await repository.DeleteAsync(id);
            if (deletedEntity is null)
            {
                return BadRequest("User not found");
            }
            accountService.Blacklist();
            return Ok(DTO(deletedEntity));
        }

        [HttpPost("newuser/new"), AllowAnonymous]
        public override async Task<ActionResult<UserDto?>> CreateAsync([FromBody] Credentials request)
        {
            // needs to be taken out of
            if (repository.AlreadyExists(request.email))
            {
                return BadRequest("Email address is taken, please try another.");
            }

            credentialsService.CreatePasswordHash(request.password, out byte[] passwordHash, out byte[] passwordSalt);

            var newUser = new UserEntity
            {
                EmailAddress = request.email,
                UserName = request.username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };

            var newEntity = await repository.AddAsync(newUser);
            return (newEntity is not null) ? DTO(newEntity) : BadRequest("Failed to create user");
        }

        public override async Task<ActionResult<UserDto?>> UpdateAsync([FromRoute] int id, UserDto updates)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost("newuser/authenticate"), AllowAnonymous]
        public async Task<ActionResult<string>> AuthenticateAsync(Credentials request)
        {
            var user = await repository.FindAsync(request.email);
            if (user is null || !credentialsService.VerifyPasswordHash(request.password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Invalid credentials");
            }
        
            var token = credentialsService.CreateToken(user);

            return Ok(token);
        }
    }
}