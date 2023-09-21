using System;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Interfaces;
using BlogAPI.PostgreSQL;
using BlogAPI.Models;

namespace BlogAPI.Repositories
{    
    public class EFUserRepository : RepositoryBase<UserEntity, BlogContext>
    {
        private readonly BlogContext context;
        public EFUserRepository(BlogContext context) : base(context)
        {
            this.context = context;
        }
        // add user specific here
        public async Task<UserEntity> FindAsync(string identifier) // email address
        {
            return await context.Set<UserEntity>().FirstOrDefaultAsync(u => u.EmailAddress == identifier);
        }
        public async Task<bool> Exists(string identifier) // email address
        {
            if (identifier.GetType() != typeof(string))
            {
                return true;
            }
            return await context.Users.AnyAsync(u => u.EmailAddress == identifier) == true;
        }
    }
}
