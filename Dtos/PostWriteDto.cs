
using BlogAPI.Models;

namespace BlogAPI.Dtos
{
    public class PostWriteDto : IDto
    {
        public string? Title { get; set; }
        public string? Body { get; set; }
        public bool IsPrivate { get; set; }
        public int AuthorId { get; set; }
    }
}