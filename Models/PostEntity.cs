
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlogAPI.Interfaces;

namespace BlogAPI.Models
{
    public class PostEntity : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public bool IsPrivate { get; set; }
        public UserEntity Author { get; set; }
    }
}