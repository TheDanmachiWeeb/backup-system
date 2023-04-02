using BackupSystem.Models;
using BackupSystem.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Org.BouncyCastle.Math.EC.ECCurve;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackupSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackupSourcesController : ControllerBase
    {
        private MyContext context = new MyContext();

        // GET api/<BackupSourcesController>/5
        [HttpGet("{configId}")]
        public async Task<ActionResult> Get(int configId)
        {
            var config = await context.Configurations.FindAsync(configId);
            if (config == null)
            {
                return BadRequest("Config with this id doesn't exist.");
            }

            var sources = await context.BackupSources
                .Where(s => s.ConfigId == configId)
                .Select(s => new BackupSourceDto { SourcePath = s.SourcePath })
                .ToListAsync();

            return Ok(sources);
        }

        // POST api/<BackupSourcesController>/5
        [HttpPost("{configId}")]
        public async Task<ActionResult> Post(int configId, [FromBody] List<BackupSourceDto> req)
        {
            var config = await context.Configurations.FindAsync(configId);
            if (config == null)
            {
                return BadRequest("Config with this id doesn't exist.");
            }

            var sources = req.Select(s => new BackupSource
            {
                ConfigId = configId,
                SourcePath = s.SourcePath
            });

            await context.AddRangeAsync(sources);

            await context.SaveChangesAsync();

            return Ok(sources);

        }

        // PUT api/<BackupSourcesController>/5
        [HttpPut("{configId}")]
        public async Task<ActionResult> Put(int configId, [FromBody] List<BackupSourceDto> sources)
        {
            var config = await context.Configurations.FindAsync(configId);
            if (config == null)
            {
                return BadRequest("Config with this id doesn't exist.");
            }

            // remove old sources
            var oldSources = await context.BackupSources.Where(s => s.ConfigId == configId).ToListAsync();
            context.RemoveRange(oldSources);

            var newSources = sources.Select(s => new BackupSource
            {
                ConfigId = configId,
                SourcePath = s.SourcePath
            });

            await context.AddRangeAsync(newSources);

            return Ok(newSources);
        }

        // DELETE api/<BackupSourcesController>/5
        [HttpDelete("{configId}")]
        public async Task<ActionResult> Delete(int configId, [FromBody] List<BackupSourceDto> sources)
        {
            var config = await context.Configurations.FindAsync(configId);
            if (config == null)
            {
                return BadRequest("Config with this id doesn't exist.");
            }

            var sourcePaths = sources.Select(dto => dto.SourcePath).ToList();
            var toRemove = await context.BackupSources
                .Where(s => s.ConfigId == configId && sourcePaths.Contains(s.SourcePath))
                .ToListAsync();

            context.BackupSources.RemoveRange(toRemove);
            await context.SaveChangesAsync();

            return Ok(toRemove);
        }
    }
}
