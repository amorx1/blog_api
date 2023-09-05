using System;
using BlogAPI.Interfaces;
namespace BlogAPI.Models
{
    public class PostDto {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public bool IsPrivate { get; set; }
        public int AuthorId { get; set; }
    }
}