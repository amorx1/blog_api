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
    [Route("api/[controller]")]
    [ApiController]
    public class NewUserController : BaseController<UserEntity, UserDto, EFUserRepository>
    {
        private readonly EFUserRepository repository;
        private readonly ICredentialsService credentialsService;
        public NewUserController(EFUserRepository repository, IAccountService accountService, ICredentialsService credentialsService)
        : base(repository, accountService, credentialsService)
        {
            this.repository = repository;
            this.credentialsService = credentialsService;
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

        // public override UserEntity? MergeUpdates(UserEntity entity, UserDto dto)
        // {
        //     entity.UserName = dto.UserName;
        //     return entity;
        // }

        // public override async Task<ActionResult<UserDto?>> CreateAsync(Credentials creds)
        // {
        //     if (repository.CheckExists(entity.EmailAddress))
        //     {
        //         return BadRequest("Email address is taken, please try another.");
        //     }

        //     credentialsService.CreatePasswordHash(entity.password, out byte[] passwordHash, out byte[] passwordSalt);

        //     var newUser = new UserEntity
        //     {
        //         EmailAddress = entity.email,
        //         UserName = request.username,
        //         PasswordHash = passwordHash,
        //         PasswordSalt = passwordSalt,
        //     };

        //     var success = await repository.AddAsync(newUser);
        //     return success ? Ok(newUser.AsDto()) : Problem("Error creating user");
        // }
    }
}