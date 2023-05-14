using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Mvc;
using BackupSystem.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackupSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private MyContext context = new MyContext();

        // POST api/<SessionsController>
        [HttpPost]
        public async Task<ActionResult> Post(User user)
        {
            try
            {
                User login = await this.context.Users.Where(x => x.Username == user.Username).FirstAsync();

                if (login.PasswordHash == user.PasswordHash)
                {
                    string token = JwtBuilder.Create()
                      .WithAlgorithm(new HMACSHA256Algorithm())
                      .WithSecret("backpussy69")
                      .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                      .AddClaim("login", user.Username)
                      .Encode();

                    return Ok(new { token = token });
                }

                return Unauthorized(new { message = "Invalid credentials" });
            }
            catch
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }
        }
    }
}
