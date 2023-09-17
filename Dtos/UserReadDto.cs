
using BlogAPI.Models;

namespace BlogAPI.Dtos
{
    public class UserReadDto : IDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
    }
}