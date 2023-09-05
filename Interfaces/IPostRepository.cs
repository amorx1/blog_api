using System;
using BlogAPI.Models;
using BlogAPI.PostgreSQL;

namespace BlogAPI.Interfaces
{
	public interface IPostRepository
	{
		Task<bool> DeletePost(int postId, int authorId);
		Task<PostDto?> AddPost(PostDto post);
		Task<PostDto?> GetPost(int postId);
		Task<PostEntity?> GetPostEntity(int postId);
		Task<PostDto?> UpdatePost(int postId, PostDto updates);
	}
}

