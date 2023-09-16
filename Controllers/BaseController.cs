using System;
using BlogAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Models;
using System.Net.Sockets;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController<TEntity, TDto, TDtoCredentials, TRepository> : ControllerBase
    where TEntity : class, IEntity
    where TDto : class, IDto
    where TDtoCredentials: class, IDto
    where TRepository: IRepository<TEntity>
    {
        private readonly TRepository repository;
        private readonly IAccountService accountService;
        private readonly ICredentialsService credentialsService;

        public BaseController(TRepository repository, IAccountService accountService, ICredentialsService credentialsService)
        {
            this.repository = repository;
            this.accountService = accountService;
            this.credentialsService = credentialsService;
        }
        public abstract TDto? DTO(TEntity entity);

        // GET
        public abstract Task<ActionResult<TDto?>> GetAsync(int id);
        public abstract Task<ActionResult<TDto?>> RemoveAsync(int id);
        public abstract Task<ActionResult<TDto?>> UpdateAsync(int id, TDto updates);
        public abstract Task<ActionResult<TDto?>> CreateAsync(Credentials request);
        
        // GET
        // [HttpGet("{id}"), Authorize]
        // [AuthenticationFilter]
        // public async Task<ActionResult<TDto?>> GetAsync(int id)
        // {
        //     var entity = await repository.GetAsync(id);
        //     return entity is null ? BadRequest("Does not exist!") : Ok(DTO(entity));
        // }
        // public abstract Task<ActionResult<TDto?>> CreateAsync(TDto entity);
        
        // [HttpDelete("user/{id}"), Authorize]
        // [AuthenticationFilter]
        // public async Task<ActionResult<TDto?>> RemoveAsync(int id)
        // {
        //     var deletedEntity = await repository.DeleteAsync(id);
        //     if (deletedEntity is null)
        //     {
        //         return BadRequest("User not found");
        //     }
        //     // override this for user controller
        //     // accountService.Blacklist();
        //     return Ok(DTO(deletedEntity));
        // }

        // GET: api/[controller]/5

        // [HttpPut("user/{id}"), Authorize]
        // public async Task<ActionResult<TDto?>> UpdateAsync(int id, TDto updates)
        // {
        //     if (id == updates.Id && await accountService.ResolveUser(id))
        //     {
        //         var entity = MergeUpdates(await repository.GetAsync(id), updates);
        //         var updatedEntity = await repository.UpdateAsync(entity);
        //         return updatedEntity is null ? BadRequest("User doesn't exist") : Ok(DTO(updatedEntity));
        //     }
        //     return Unauthorized("Access denied");
        // }   

        // public abstract Task<ActionResult<TDto?>> CreateAsync(TDto creds);
        // [HttpPost("user/new"), AllowAnonymous]
        // public async Task<ActionResult<TDto?>> CreateAsync(Credentials request)
        // {
        //     // needs to be taken out of
        //     if (repository.EmailExists(request.email))
        //     {
        //         return BadRequest("Email address is taken, please try another.");
        //     }

        //     credentialsService.CreatePasswordHash(request.password, out byte[] passwordHash, out byte[] passwordSalt);

        //     var newUser = new UserEntity
        //     {
        //         EmailAddress = request.email,
        //         UserName = request.username,
        //         PasswordHash = passwordHash,
        //         PasswordSalt = passwordSalt,
        //     };

        //     var success = await repository.AddAsync(newUser);
        //     return success ? Ok(newUser.AsDto()) : Problem("Error creating user");
        // }
    }
}