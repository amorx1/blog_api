
using BlogAPI.Models;

namespace BlogAPI.Dtos
{
    /*
    Data transfer object used for creating/updating users (Id auto-generated).
    */
    public class UserWriteDto : IDto
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public ICollection<PostWriteDto>? Posts { get; set; }
    }
}