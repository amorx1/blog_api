using BlogAPI.Interfaces;
using BlogAPI.Models;

namespace BlogAPI.Dtos
{
	/*
	Data transfer object for writing images to database.
	*/
	public class ImageWriteDto : IDto
	{
		public string Uri { get; set; }
		public int PostId { get ; set; }
	}
}
