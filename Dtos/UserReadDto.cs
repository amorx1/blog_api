
using BlogAPI.Models;

namespace BlogAPI.Dtos
{
    /*
    Data transfer object used for providing user data without sensitive data.
    */
    public class UserReadDto : IDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public ICollection<PostReadDto>? Posts { get; set; }
    }
}