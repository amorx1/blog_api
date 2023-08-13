using System;
using BlogAPI.Models;
using BlogAPI.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Repositories
{
	public class PostRepository : IPostRepository
	{
		private readonly BlogContext _db;

		public PostRepository(BlogContext db)
		{
			_db = db;
		}

        public async Task<PostDto?> GetPost(int postId)
        {
			var post = await _db.Posts.FindAsync(postId);
            return post.AsDto();
        }

		public async Task<PostEntity?> GetPostEntity(int postId)
		{
			return await _db.Posts.FindAsync(postId);
		}


        public async Task<bool> DeletePost(int postId, int authorId)
		{
			var post = await _db.Posts.FirstOrDefaultAsync(p => p.Author.Id == authorId && p.Id == postId);	
			try
			{
				_db.Posts.Remove(post);
				await _db.SaveChangesAsync();

				return true;
			}
			catch
			{
				return false;
			}
		}

		public async Task<PostDto?> AddPost(PostDto post)
		{
            var author = await _db.Users.FindAsync(post.AuthorId);
            var newPost = new PostEntity
            {
                Title = post.Title,
                Body = post.Body,
                Author = author,
                IsPrivate = post.IsPrivate
            };
            try
			{
				await _db.Posts.AddAsync(newPost);
				await _db.SaveChangesAsync();

				return newPost.AsDto();
			}
			// nullreferenceexception
			catch
			{
				return null;
			}
		}

        public async Task<PostDto?> UpdatePost(int postId, PostDto request)
		{
			var post = await GetPostEntity(postId);
			try
			{
				post.Title = request.Title;
				post.Body = request.Body;
				post.IsPrivate = request.IsPrivate;

				_db.Posts.Update(post);
				await _db.SaveChangesAsync();

				return post.AsDto();
			}
			catch
			{
				return await GetPost(postId);
			}
		}
	}
}

