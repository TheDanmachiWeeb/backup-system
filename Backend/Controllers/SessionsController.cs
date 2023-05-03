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
        // GET: api/<SessionsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<SessionsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

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
                      .WithSecret("super-secret-foobar")
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

        // PUT api/<SessionsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SessionsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
