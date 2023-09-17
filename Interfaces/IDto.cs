using System;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Models
{
    public interface IDto
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
    }
}
