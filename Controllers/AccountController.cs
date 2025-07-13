using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XSLearning.DTOs;
using XSLearning.Services;

namespace XSLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Authenticates a user and returns their details
        /// </summary>
        /// <param name="userDto">Username and password credentials</param>
        /// <returns>User information on successful login</returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserResponseDto>> Login(UserDto userDto)
        {
            var user = await _userService.AuthenticateAsync(userDto.Username, userDto.Password);

            if (user == null)
                return Unauthorized(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="userDto">Registration details</param>
        /// <returns>User information on successful registration</returns>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponseDto>> Register(RegisterUserDto userDto)
        {
            var user = await _userService.RegisterAsync(userDto);

            if (user == null)
                return BadRequest(new { message = "Username already exists" });

            return CreatedAtAction(nameof(Register), user);
        }
    }
}
