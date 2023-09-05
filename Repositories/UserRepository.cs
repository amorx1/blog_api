using BlogAPI.Interfaces;
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

		public async Task<bool> DeleteUser(int userId)
		{
			var user = await GetUser(userId);
			try
			{
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();

				return true;
            }
			catch
			{
				return false;
			}

		}

		public async Task<UserEntity?> GetUser(int userId)
		{
            return await _db.Users.FindAsync(userId);
        }

		public async Task<bool> AddUser(UserEntity user)
		{
			try
			{
                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();
				return true;
            }
			catch
			{
				return false;
			}

		}

		public async Task<UserEntity?> UpdateUser(int userId, UserDto updates)
		{
			var user = await GetUser(userId);
			try
			{
                user.UserName = updates.UserName; // updates to Id or EmailAddress not allowed
                _db.Users.Update(user);
                await _db.SaveChangesAsync();

				return user;
            }
			catch
			{
				return await GetUser(userId);
			}
		}

		public bool EmailExists(string requestEmail) => _db.Users.Any(u => u.EmailAddress == requestEmail) == true;

    }
}

