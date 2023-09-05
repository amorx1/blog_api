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
        public bool CheckExists(string identifier)
        {
            if (identifier.GetType() != typeof(string))
            {
                return true;
            }
            return context.Set<UserEntity>().Any(u => u.EmailAddress == identifier) == true;
        }
    }
}
