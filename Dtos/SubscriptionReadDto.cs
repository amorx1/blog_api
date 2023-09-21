using BlogAPI.Models;

namespace BlogAPI.Dtos
{
	public class SubscriptionReadDto : IDto
	{
		public int Id { get; set; }
		public int ToUserId { get; set; }
	}
}
