using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using BlogAPI.Interfaces;

namespace BlogAPI.Models
{
	public class SubscriptionEntity : IEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		public int FromUserId { get; set; }
		[Required]
		public int ToUserId { get; set; }

		[ForeignKey("FromUserId")]
		public virtual UserEntity FromUser { get; set; }
		[ForeignKey("ToUserId")]
		public virtual UserEntity ToUser { get; set; }
	}
}
