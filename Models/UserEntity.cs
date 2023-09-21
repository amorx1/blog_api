using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlogAPI.Interfaces;

namespace BlogAPI.Models
{
    public class UserEntity : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public ICollection<PostEntity> Posts { get; set; } = new List<PostEntity>();

        public virtual ICollection<SubscriptionEntity>? Following { get; set; } = new List<SubscriptionEntity>();
        public virtual ICollection<SubscriptionEntity>? Followers { get; set; } = new List<SubscriptionEntity>();
    
    }
}