using BackupSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackupSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private MyContext context = new MyContext();

        // GET: api/<UsersController>
        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            return Ok(await context.Users.ToListAsync());
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<List<User>>> Get(int id)
        {
            var user = await context.Users.FindAsync(id);

            if (user == null)
                return NotFound("User with this ID not found.");

            return Ok(user);
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<ActionResult<List<User>>> Post([FromBody] User req)
        {
            var userDb = await context.Users.Where(u => u.Username == req.Username).FirstOrDefaultAsync();
            if (userDb != null)
                return BadRequest("User already exists.");

            context.Users.Add(req);
            await context.SaveChangesAsync();

            return Ok(req);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<List<User>>> Put(int id, [FromBody] User req)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            user.Email = req.Email;
            user.Username = req.Username;
            user.PasswordHash = req.PasswordHash;

            await context.SaveChangesAsync();

            return Ok(await context.Users.ToListAsync());
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<User>>> Delete(int id)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null)
                return NotFound("User with this ID not found.");

            context.Users.Remove(user);

            await context.SaveChangesAsync();

            return Ok(await context.Users.ToListAsync());
        }
    }
}
