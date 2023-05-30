using BackupSystem.Models;
using BackupSystem.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BackupSystem.Controllers
{
    [Authorize(admin = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class BackupDestinationsController : ControllerBase
    {
        private MyContext context = new MyContext();

        // GET api/<BackupDestinationsController>/5
        [HttpGet("{configId}")]
        public async Task<ActionResult> Get(int configId)
        {
            var config = await context.Configurations.FindAsync(configId);
            if (config == null)
            {
                return BadRequest("Config with this id doesn't exist.");
            }

            var destinations = await context.BackupDestinations
                .Where(d => d.ConfigId == configId)
                .Select(d => new BackupDestinationDto { Path = d.DestinationPath, Type = d.DestinationType })
                .ToListAsync();

            return Ok(destinations);
        }

        // POST api/<BackupDestinationsController>/5
        [HttpPost("{configId}")]
        public async Task<ActionResult> Post(int configId, [FromBody] List<BackupDestinationDto> req)
        {
            var config = await context.Configurations.FindAsync(configId);
            if (config == null)
            {
                return BadRequest("Config with this id doesn't exist.");
            }

            var destinations = req.Select(d => new BackupDestination
            {
                ConfigId = configId,
                DestinationPath = d.Path,
                DestinationType = d.Type
            });

            await context.AddRangeAsync(destinations);
            await context.SaveChangesAsync();

            return Ok(destinations);
        }

        // PUT api/<BackupDestinationsController>/5
        [HttpPut("{configId}")]
        public async Task<ActionResult> Put(int configId, [FromBody] List<BackupDestinationDto> destinations)
        {
            var config = await context.Configurations.FindAsync(configId);
            if (config == null)
            {
                return BadRequest("Config with this id doesn't exist.");
            }

            // remove old destinations
            var oldDestinations = await context.BackupDestinations.Where(d => d.ConfigId == configId).ToListAsync();
            context.RemoveRange(oldDestinations);

            var newDestinations = destinations.Select(d => new BackupDestination
            {
                ConfigId = configId,
                DestinationPath = d.Path,
                DestinationType = d.Type
            });

            await context.AddRangeAsync(newDestinations);
            await context.SaveChangesAsync();

            return Ok(newDestinations);
        }

        // DELETE api/<BackupDestinationsController>/5
        [HttpDelete("{configId}")]
        public async Task<ActionResult> Delete(int configId, [FromBody] List<BackupDestinationDto> destinations)
        {
            var config = await context.Configurations.FindAsync(configId);
            if (config == null)
            {
                return BadRequest("Config with this id doesn't exist.");
            }

            var destinationPaths = destinations.Select(dto => dto.Path).ToList();
            var toRemove = await context.BackupDestinations
                .Where(d => d.ConfigId == configId && destinationPaths.Contains(d.DestinationPath))
                .ToListAsync();

            context.BackupDestinations.RemoveRange(toRemove);
            await context.SaveChangesAsync();

            return Ok(toRemove);
        }
    }
}