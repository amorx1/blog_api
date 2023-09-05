using System;
using BlogAPI.Interfaces;

namespace BlogAPI.Models
{
	public class UserDto : IDto {
		public int Id { get; set; }
		public string UserName { get; set; }
		public string EmailAddress { get; set; }
	}
}

