using System;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Interfaces;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BlogAPI.Repositories
{
    public abstract class RepositoryBase<TEntity, TContext> : IRepository<TEntity> 
    where TEntity : class, IEntity
    where TContext : DbContext
    {
        private readonly TContext context;
        public RepositoryBase(TContext context)
        {
            this.context = context;
        }

        public async Task<TEntity?> AddAsync(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity?> UpdateAsync(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity?> DeleteAsync(int id)
        {
            var entity = await context.Set<TEntity>().FindAsync(id);
            if (entity is null) {
                return entity;
            }

            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<TEntity?> GetAsync(int id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<List<TEntity>> GetAllAsync(int userId)
        {
            return await context.Set<TEntity>().Where(e => e.Id == userId).ToListAsync();
        }
    }
}
