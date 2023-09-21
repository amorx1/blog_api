using BlogAPI.Models;

namespace BlogAPI.Dtos
{
	public class SubscriptionWriteDto : IDto
	{
		public int FromUserId { get; set; }
		public int ToUserId { get; set; }
	}
}
