using System;
using BlogAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Models;
using BlogAPI.Repositories;
using BlogAPI.PostgreSQL;
using BlogAPI.Dtos;
using AutoMapper;
using BlogApi;

namespace BlogAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : BaseController<UserEntity, UserReadDto, UserWriteDto, EFUserRepository>
    {
        private readonly EFUserRepository _repository;
        private readonly ICredentialsService _credentialsService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public UserController(EFUserRepository repository, IAccountService accountService, ICredentialsService credentialsService, IMapper mapper)
        : base(repository, accountService, credentialsService, mapper)
        {
            this._repository = repository;
            this._credentialsService = credentialsService;
            this._accountService = accountService;
            this._mapper = mapper;
        }

        [HttpGet("user/{id}"), Authorize, AuthenticationFilter]
        public override async Task<ActionResult<UserReadDto?>> GetAsync([FromRoute] int id)
        {
            var user = await _repository.GetAsync(id);
            return user is null ? BadRequest("Does not exist!") : Ok(_mapper.Map<UserReadDto>(user));
        }

        [HttpDelete("user/{id}"), Authorize, AuthenticationFilter]
        public override async Task<ActionResult<UserReadDto?>> RemoveAsync([FromRoute] int id)
        {
            var deletedEntity = await _repository.DeleteAsync(id);
            if (deletedEntity is null)
            {
                return BadRequest("User not found");
            }
            _accountService.Blacklist();
            return Ok(_mapper.Map<UserReadDto>(deletedEntity));
        }

        [HttpPost("user/new"), AllowAnonymous, AssertUnauthenticatedFilter] // prevent user creation when already authenticated
        public override async Task<ActionResult<UserReadDto?>> CreateAsync([FromBody] UserWriteDto request)
        {
            // needs to be taken out of
            var emailTaken = _repository.Exists(request.EmailAddress);

            _credentialsService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var newUser = _mapper.Map<UserEntity>(request, opt => 
            {
                opt.AfterMap((source, destination) =>
                {
                    destination.PasswordHash = passwordHash;
                    destination.PasswordSalt = passwordSalt;
                });
            });

            if (await emailTaken)
            {
                return BadRequest("Failed to create user. Email address already exists.");
            }

            var newEntity = await _repository.AddAsync(newUser);
            return (newEntity is not null) ? _mapper.Map<UserReadDto>(newEntity) : BadRequest("Failed to create user");
        }

        [HttpPut("user/{id}"), Authorize, AuthenticationFilter]
        public override async Task<ActionResult<UserReadDto?>> UpdateAsync([FromRoute] int id, UserWriteDto updates)
        {
            var user = await _repository.GetAsync(id);
            if (user is null)
            {
                return BadRequest("User does not exist");
            }

            // merge updates
            user.UserName = updates.UserName;
            var updatedUser = await _repository.UpdateAsync(user);

            return Ok(_mapper.Map<UserReadDto>(updatedUser));
        }
        
        [HttpPost("user/authenticate"), AllowAnonymous]
        public async Task<ActionResult<string>> AuthenticateAsync([FromBody] UserWriteDto request)
        {
            var user = await _repository.FindAsync(request.EmailAddress);
            if (user is null || !_credentialsService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Invalid credentials");
            }
        
            var token = _credentialsService.CreateToken(user);

            return Ok(token);
        }
    }
}