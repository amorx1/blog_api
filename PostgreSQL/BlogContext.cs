using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using BlogAPI.Interfaces;
using BlogAPI.Models;

namespace BlogAPI.PostgreSQL
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options){}
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<PostEntity> Posts { get; set; }
    }
}