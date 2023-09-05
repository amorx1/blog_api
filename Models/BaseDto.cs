using BlogAPI.Interfaces;

namespace BlogAPI.Models
{
    public abstract class BaseDto : IDto {
        public int Id { get; set; }
    }
}