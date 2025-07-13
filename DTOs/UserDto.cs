using System.ComponentModel.DataAnnotations;

namespace XSLearning.DTOs
{
    /// <summary>
    /// DTO for user authentication requests only.
    /// IMPORTANT: This DTO contains sensitive information and should NEVER be used for responses.
    /// </summary>
    public class UserDto
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
    }

    /// <summary>
    /// DTO for user information responses. Contains no sensitive data.
    /// Use this for API responses when returning user information.
    /// </summary>
    public class UserResponseDto
    {
        public string Username { get; set; }
        public string Role { get; set; } = "User";
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
    }
    
    /// <summary>
    /// DTO for user registration requests.
    /// IMPORTANT: This DTO contains sensitive information and should NEVER be used for responses.
    /// </summary>
    public class RegisterUserDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        public string Password { get; set; }
        
        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
