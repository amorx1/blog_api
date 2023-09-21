using BlogAPI.Models;

namespace BlogAPI.Dtos
{
	/*
	Data transfer object for reading images from database.
	*/
	public class ImageReadDto : IDto
	{
		public int Id { get; set; }
		public string Uri { get; set; }
		public int PostId { get; set; }
	}
}