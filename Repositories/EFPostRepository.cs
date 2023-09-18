using BlogAPI.Models;
using BlogAPI.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Repositories
{
	public class EFPostRepository : RepositoryBase<PostEntity, BlogContext>
	{
		private readonly BlogContext _context;

		public EFPostRepository(BlogContext context) : base(context)
		{
			_context = context;
		}
		// post specific db methods here

		public override async Task<List<PostEntity>> GetAllAsync(int userId)
        {
            return await _context.Set<PostEntity>().Where(p => p.Author.Id == userId).ToListAsync();
        }	
		public override async Task<PostEntity?> GetAsync(int id)
		{
			// Here .Include() is used to prevent lazy loading which would cause Author information to be omitted.
			return await _context.Posts.Include(p => p.Author).FirstOrDefaultAsync(p => p.Id == id);
		}
	}
}
