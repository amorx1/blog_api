using System;
using System.Data.Entity;
using BlogAPI.Models;
using BlogAPI.PostgreSQL;

namespace BlogAPI.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly BlogContext _db;

		public UserRepository(BlogContext db)
		{
			_db = db;
		}

		public async void DeleteUser(int id)
		{
			var user = await GetUser(id);
			_db.Users.Remove(user);
			await _db.SaveChangesAsync();

			return;
		}

		public async Task<UserEntity?> GetUser(int id)
		{
            return await _db.Users.FindAsync(id);
        }

		public async void AddUser(UserEntity user)
		{
			await _db.Users.AddAsync(user);
			await _db.SaveChangesAsync();

			return;
		}

		public async Task<UserEntity?> UpdateUser(int id, UserDto updates)
		{
			var user = await _db.Users.FindAsync(id);
			if (user == null)
			{
				return null;
			}
			user.UserName = updates.UserName; // updates to Id or EmailAddress not allowed
			_db.Users.Update(user);
			await _db.SaveChangesAsync();

			return user;
		}

		public bool EmailExists(string requestEmail) => (_db.Users.Any(u => u.EmailAddress == requestEmail) == true);

    }
}

