using System;
using BlogAPI.Models;
using BlogAPI.PostgreSQL;

namespace BlogAPI.Repositories
{
	public interface IUserRepository
	{
		bool EmailExists(string requestEmail);
        Task<bool> DeleteUser(int userId);
        Task<bool> AddUser(UserEntity user);
        Task<UserEntity?> GetUser(int userId);
		Task<UserEntity?> UpdateUser(int userId, UserDto updates);
	}
}

