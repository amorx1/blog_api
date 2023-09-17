using System;
using BlogAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Models;
using System.Net.Sockets;
using AutoMapper;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController<TEntity, TReadDto, TWriteDto, TRepository> : ControllerBase
    where TEntity : class, IEntity
    where TReadDto : class, IDto
    where TWriteDto: class, IDto
    where TRepository: IRepository<TEntity>
    {
        private readonly TRepository _repository;
        private readonly IAccountService _accountService;
        private readonly ICredentialsService _credentialsService;
        private readonly IMapper _mapper;

        public BaseController(TRepository repository, IAccountService accountService, ICredentialsService credentialsService, IMapper mapper)
        {
            this._repository = repository;
            this._accountService = accountService;
            this._credentialsService = credentialsService;
            this._mapper = mapper;
        }

        public abstract Task<ActionResult<TReadDto?>> GetAsync(int id);
        public abstract Task<ActionResult<TReadDto?>> RemoveAsync(int id);
        public abstract Task<ActionResult<TReadDto?>> UpdateAsync(int id, TWriteDto updates);
        public abstract Task<ActionResult<TReadDto?>> CreateAsync(TWriteDto request);
    }
}