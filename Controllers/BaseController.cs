using BlogAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BlogAPI.Models;
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
        private readonly IMapper _mapper;

        public BaseController(TRepository repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        // TODO: Provide generic implementations and override if necessary in concrete controllers.
        // TODO: Define delegates for user/post specific actions.
        public virtual Task<ActionResult<TReadDto?>> GetAsync(int id)
        {
            throw new NotImplementedException();
        }
        public virtual Task<ActionResult<TReadDto?>> GetAsync(int id1, int id2)
        {
            throw new NotImplementedException();
        }
        public virtual Task<ActionResult<TReadDto?>> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }
        public virtual Task<ActionResult<TReadDto?>> RemoveAsync(int id1, int id2)
        {
            throw new NotImplementedException();
        }
        public virtual Task<ActionResult<TReadDto?>> UpdateAsync(int id, TWriteDto updates)
        {
            throw new NotImplementedException();
        }
        public virtual Task<ActionResult<TReadDto?>> UpdateAsync(int id1, int id2, TWriteDto updates)
        {
            throw new NotImplementedException();
        }
        public virtual Task<ActionResult<TReadDto?>> CreateAsync(TWriteDto request)
        {
            throw new NotImplementedException();
        }
        public virtual Task<ActionResult<TReadDto?>> CreateAsync(int id, TWriteDto request)
        {
            throw new NotImplementedException();
        }
    }
}