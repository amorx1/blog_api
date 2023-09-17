
using BlogAPI.Models;

namespace BlogAPI.Dtos
{
    public class UserWriteDto : IDto
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}