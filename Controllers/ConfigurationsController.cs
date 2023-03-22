using Microsoft.AspNetCore.Mvc;
using BackupSystem.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackupSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationsController : ControllerBase
    {
        private MyContext context = new MyContext();
        // GET: api/<ConfigurationsController>
        [HttpGet]
        public async Task<ActionResult<List<Configuration>>> Get()
        {
            return await context.Configurations.ToListAsync();
        }

        // GET api/<ConfigurationsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Configuration>> Get(int id)
        {
            var configuration = await context.Configurations.FindAsync(id);

            if (configuration == null)
                return NotFound();

            return configuration;
        }

        // POST api/<ConfigurationsController>
        [HttpPost]
        public async Task<ActionResult<Configuration>> Post([FromBody] Configuration req)
        {
            // Create new Configuration record
            var config = new Configuration
            {
                BackupType = req.BackupType,
                Retention = req.Retention,
                PackageSize = req.PackageSize,
                PeriodCron = req.PeriodCron
            };

            context.Configurations.Add(config);

            // Create new BackupSources records
            foreach (BackupSource source in req.BackupSources)
            {
                var backupSource = new BackupSource
                {
                    ConfigId = config.ConfigId,
                    SourcePath = source.SourcePath
                };

                context.BackupSources.Add(backupSource);
            }

            // Create new BackupDestinations records
            foreach (BackupDestination destination in req.BackupDestinations)
            {
                var backupDestination = new BackupDestination
                {
                    ConfigId = config.ConfigId,
                    DestinationType = destination.DestinationType,
                    DestinationPath = destination.DestinationPath
                };

                context.BackupDestinations.Add(backupDestination);
            }

            await context.SaveChangesAsync();

            // Return the newly created Configuration record
            return CreatedAtAction(nameof(Get), new { id = config.ConfigId }, config);
        }

        // PUT api/<ConfigurationsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Configuration>> Put(int id, [FromBody] Configuration req)
        {
            var configuration = await context.Configurations.FindAsync(id);

            if (configuration == null)
                return NotFound();

            configuration.BackupType = req.BackupType;
            configuration.Retention = req.Retention;
            configuration.PackageSize = req.PackageSize;
            configuration.PeriodCron = req.PeriodCron;

            context.Entry(configuration).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return Ok(configuration);
        }

        // DELETE api/<ConfigurationsController>/5
        [HttpDelete("{id}")]
        //public async Task<ActionResult<Configuration>> Delete(int id)
        //{
        //}

        private bool ConfigurationExists(int id)
        {
            return context.Configurations.Any(config => config.ConfigId == id);
        }
    }
}
