using System.ComponentModel.DataAnnotations;

namespace XSLearning.Models
{
    public class Users
    {
        [Key]
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
