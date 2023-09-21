using BlogAPI.Models;
using BlogAPI.PostgreSQL;

namespace BlogAPI.Repositories
{
	public class EFImageRepository : RepositoryBase<ImageEntity, BlogContext>
	{
		private readonly BlogContext _context;

		public EFImageRepository(BlogContext context) : base(context)
		{
			_context = context;
		}

		public async Task<List<ImageEntity?>> AddBatchAsync(List<ImageEntity> images)
		{
			await _context.Images.AddRangeAsync(images);
			await _context.SaveChangesAsync();
			return images;
		}
	}
}
