using BlogAPI.Interfaces;
using BlogAPI.Models;

namespace BlogAPI.Dtos
{
    /*
    Data transfer object for reading blog posts data
    */
    public class PostReadDto : IDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        // TODO: not sure if want to display this yet
        // public bool IsPrivate { get; set; }
        public int AuthorId { get; set; }
        public ICollection<ImageReadDto>? Images { get; set; }
    }
}