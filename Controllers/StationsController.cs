using BackupSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackupSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationsController : ControllerBase
    {
        private MyContext context = new MyContext();

        // GET: api/<StationsController>
        [HttpGet]
        public async Task<ActionResult<List<Station>>> Get()
        {
            return Ok(await context.Stations.ToListAsync());
        }

        // GET api/<StationsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Station>>> Get(int id)
        {
            var station = await context.Stations.FindAsync(id);

            if (station == null)
                return NotFound("Station with this ID not found.");

            return Ok(station);
        }

        // POST api/<StationsController>
        [HttpPost]
        public async Task<ActionResult<List<Station>>> Post([FromBody] Station req)
        {
            var stationDb = await context.Stations.Where(s => s.MacAddress == req.MacAddress).FirstOrDefaultAsync();
            if (stationDb != null)
                return BadRequest("Station already exists.");

            context.Stations.Add(req);
            await context.SaveChangesAsync();

            return Ok(req);
        }

        // PUT api/<StationsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<List<Station>>> Put(int id, [FromBody] Station req)
        {
            var station = await context.Stations.FindAsync(id);
            if (station == null)
                return NotFound();

            station.StationName = req.StationName;
            station.IpAddress = req.IpAddress;
            station.MacAddress = req.MacAddress;
            station.Active = req.Active;

            await context.SaveChangesAsync();

            return Ok(await context.Stations.ToListAsync());
        }

        // DELETE api/<StationsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Station>>> Delete(int id)
        {
            var station = await context.Stations.FindAsync(id);
            if (station == null)
                return NotFound("Station with this ID not found.");

            context.Stations.Remove(station);

            await context.SaveChangesAsync();

            return Ok(await context.Stations.ToListAsync());
        }
    }
}
