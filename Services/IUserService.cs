using System.Collections.Generic;
using System.Threading.Tasks;
using XSLearning.DTOs;
using XSLearning.Models;

namespace XSLearning.Services
{
    public interface IUserService
    {
        Task<UserResponseDto> AuthenticateAsync(string username, string password);
        Task<UserResponseDto> RegisterAsync(RegisterUserDto userDto);
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<UserResponseDto> GetUserByUsernameAsync(string username);
        Task<UserResponseDto> UpdateUserAsync(string username, RegisterUserDto userDto);
        Task<bool> DeleteUserAsync(string username);
    }
}
