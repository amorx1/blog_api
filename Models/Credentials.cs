using BlogAPI.Interfaces;

namespace BlogAPI.Models
{
    public class Credentials : IDto
    {
        public int Id { get; set;}
        public string email { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }
}