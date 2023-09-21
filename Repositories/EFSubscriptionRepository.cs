
using BlogAPI.Models;
using BlogAPI.PostgreSQL;

namespace BlogAPI.Repositories
{
	public class EFSubscriptionRepository : RepositoryBase<SubscriptionEntity, BlogContext>
	{
		private readonly BlogContext _context;

		public EFSubscriptionRepository(BlogContext context) : base(context)
		{
			_context = context;
		}

		// sub-specific repo methods here
	}
}
