using System;
using BlogAPI.Models;
using BlogAPI.PostgreSQL;

namespace BlogAPI.Repositories
{
	public interface IUserRepository
	{
        void AddUser(UserEntity user);
        void DeleteUser(int id);
		bool EmailExists(string requestEmail);
        Task<UserEntity?> GetUser(int id);
		Task<UserEntity?> UpdateUser(int id, UserDto updates);
	}
}

