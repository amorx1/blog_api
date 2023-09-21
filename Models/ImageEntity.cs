using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlogAPI.Interfaces;

namespace BlogAPI.Models
{
	public class ImageEntity : IEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string? Uri { get; set; }
		public int PostId { get; set; }
		public PostEntity Post { get; set; } = null!;
	}
}
