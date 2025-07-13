using System;
using System.ComponentModel.DataAnnotations;

namespace XSLearning.Models
{
    public class Users
    {
        [Key]
        public string Username { get; set; }
        
        [Required]
        public string PasswordHash { get; set; }
        
        [Required]
        public string Role { get; set; } = "User";
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastLogin { get; set; }
    }
}
