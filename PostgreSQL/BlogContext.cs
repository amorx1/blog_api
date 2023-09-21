using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using BlogAPI.Interfaces;
using BlogAPI.Models;

namespace BlogAPI.PostgreSQL
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {    
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<ImageEntity> Images { get; set; }
        public DbSet<SubscriptionEntity> Subscriptions { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubscriptionEntity>()
            .HasOne(s => s.FromUser)
            .WithMany(u => u.Following)
            .HasForeignKey(s => s.FromUserId)
            .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<SubscriptionEntity>()
            .HasOne(s => s.ToUser)
            .WithMany(u => u.Followers)
            .HasForeignKey(s => s.ToUserId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}