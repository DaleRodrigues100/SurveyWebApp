using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using XSLearning.DTOs;
using XSLearning.Models;
using XSLearning.Repositories;

namespace XSLearning.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserResponseDto> AuthenticateAsync(string username, string password)
        {
            var user = await _unitOfWork.Users.GetUserByUsernameAsync(username);
            
            // Return null if user not found
            if (user == null)
                return null;

            // Check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash))
                return null;

            // Update last login time
            user.LastLogin = DateTime.UtcNow;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.CompleteAsync();

            // Authentication successful
            return new UserResponseDto
            {
                Username = user.Username,
                Role = user.Role
            };
        }

        public async Task<UserResponseDto> RegisterAsync(RegisterUserDto userDto)
        {
            // Check if username already exists
            if (await _unitOfWork.Users.UserExistsAsync(userDto.Username))
                return null;

            // Create user entity
            var user = new Users
            {
                Username = userDto.Username,
                PasswordHash = HashPassword(userDto.Password),
                Role = "User", // Default role
                CreatedAt = DateTime.UtcNow
            };

            // Add user to database
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            // Return user DTO
            return new UserResponseDto
            {
                Username = user.Username,
                Role = user.Role
            };
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return users.Select(u => new UserResponseDto
            {
                Username = u.Username,
                Role = u.Role
            });
        }

        public async Task<UserResponseDto> GetUserByUsernameAsync(string username)
        {
            var user = await _unitOfWork.Users.GetUserByUsernameAsync(username);
            if (user == null)
                return null;

            return new UserResponseDto
            {
                Username = user.Username,
                Role = user.Role
            };
        }

        public async Task<UserResponseDto> UpdateUserAsync(string username, RegisterUserDto userDto)
        {
            var user = await _unitOfWork.Users.GetUserByUsernameAsync(username);
            if (user == null)
                return null;

            // Update user properties
            if (!string.IsNullOrWhiteSpace(userDto.Password))
            {
                user.PasswordHash = HashPassword(userDto.Password);
            }

            _unitOfWork.Users.Update(user);
            await _unitOfWork.CompleteAsync();

            return new UserResponseDto
            {
                Username = user.Username,
                Role = user.Role
            };
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            var user = await _unitOfWork.Users.GetUserByUsernameAsync(username);
            if (user == null)
                return false;

            _unitOfWork.Users.Remove(user);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        // Helper methods for password hashing
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == storedHash;
        }
    }
}
