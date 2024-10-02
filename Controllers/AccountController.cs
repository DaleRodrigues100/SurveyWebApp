using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XSLearning.Models;
namespace XSLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly DataContext dc;

        public AccountController(DataContext dc)
        {
            this.dc = dc;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(Users user)
        {

            var get_user = await dc.Users.FirstOrDefaultAsync(x => x.Username == user.Username);

            if (get_user == null || get_user.Password != user.Password)
                return Unauthorized(401);

            else
            {

                return Ok(user);

            }

        }

    }


     

}





