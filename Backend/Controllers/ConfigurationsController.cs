using Microsoft.AspNetCore.Mvc;
using BackupSystem.Models;
using BackupSystem.Dtos;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Diagnostics;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Newtonsoft.Json;
using System.Text.RegularExpressions;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackupSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationsController : ControllerBase
    {
        private MyContext context = new MyContext();


        // GET: api/<ConfigurationsController>
        [Authorize(admin = true)]
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] List<string> include)
        {
            var configs = await context.Configurations
                .Select(c => new
                {
                    ConfigId = c.ConfigId,
                    ConfigName = c.ConfigName,
                    BackupType = c.BackupType,
                    Retention = c.Retention,
                    PackageSize = c.PackageSize,
                    PeriodCron = c.PeriodCron,
                    Zip = c.Zip,
                    Periodic = c.Periodic,
                    Finished = c.Finished,
                    BackupSources = c.BackupSources
                        .Select(bs => new
                        {
                            SourcePath = bs.SourcePath
                        })
                        .ToList(),

                    BackupDestinations = c.BackupDestinations
                        .Select(bs => new
                        {
                            DestinationPath = bs.DestinationPath,
                            DestinationType = bs.DestinationType
                        })
                        .ToList(),
                    Groups = include.Contains("Groups") ? c.StationConfigurations!
                        .ToList().Select(sc => new
                        {
                            GroupId = sc.GroupId,
                            GroupName = sc.Group!.GroupName
                        }).ToList() : null,
                    Stations = include.Contains("Stations") ? c.StationConfigurations!
                        .ToList().Select(sc => new
                        {
                            StationId = sc.StationId,
                            StationName = sc.Station.StationName
                        }).ToList() : null
                }).ToListAsync();

            return Ok(configs);
        }

        // GET api/<ConfigurationsController>/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var config = await context.Configurations
                .Include(c => c.BackupSources)
                .Include(c => c.BackupDestinations)
                .Include(c => c.StationConfigurations)
                    .ThenInclude(sc => sc.Group)
                .Include(c => c.StationConfigurations)
                    .ThenInclude(sc => sc.Station)
                .FirstOrDefaultAsync(c => c.ConfigId == id);

            if (config == null)
                return NotFound();

            var result = new
            {
                ConfigId = config.ConfigId,
                ConfigName = config.ConfigName,
                BackupType = config.BackupType,
                Retention = config.Retention,
                PackageSize = config.PackageSize,
                PeriodCron = config.PeriodCron,
                Zip = config.Zip,
                Periodic = config.Periodic,
                Finished = config.Finished,
                Sources = config.BackupSources
                    .Select(bs => new
                    {
                        Path = bs.SourcePath
                    })
                    .ToList(),
                Destinations = config.BackupDestinations
                    .Select(bs => new
                    {
                        Path = bs.DestinationPath,
                        Type = bs.DestinationType
                    })
                    .ToList(),
                Groups = config.StationConfigurations.Where(sc => sc.Group != null)
                    .Select(sc => new
                    {
                        GroupId = sc.Group.GroupId,
                        GroupName = sc.Group.GroupName
                    })
                    .Distinct()
                    .ToList(),
                Stations = config.StationConfigurations.Where(sc => sc.GroupId == null)
                    .Select(sc => new
                    {
                        StationId = sc.Station.StationId,
                        StationName = sc.Station.StationName
                    })
                    .Distinct()
                    .ToList()
            };

            return Ok(result);
        }


        //POST api/<ConfigurationsController>
        [Authorize(admin = true)]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ConfigurationDto req)
        {
            // Create new Configuration record
            Configuration config = new Configuration
            {
                ConfigName = req.ConfigName,
                BackupType = req.BackupType,
                Retention = req.Retention,
                PackageSize = req.PackageSize,
                PeriodCron = req.PeriodCron,
                Zip = req.Zip,
                Periodic = req.Periodic,
                Finished = req.Finished,
            };

            context.Configurations.Add(config);

            // Create new BackupSources records
            foreach (BackupSourceDto source in req.Sources)
            {
                context.BackupSources.Add(new BackupSource
                {
                    Config = config,
                    SourcePath = source.Path
                });
            }

            // Create new BackupDestinations records
            foreach (BackupDestinationDto destination in req.Destinations)
            {
                context.BackupDestinations.Add(new BackupDestination
                {
                    Config = config,
                    DestinationPath = destination.Path,
                    DestinationType = destination.Type,
                });
            }

            await context.SaveChangesAsync();

            // Create new StationConfiguration records for Groups
            foreach (int stationId in req.Stations)
            {
                var dbStation = await context.Stations.Where(s => s.StationId == stationId).FirstOrDefaultAsync();
                if (dbStation == null)
                    return NotFound("Station not found.");

                context.StationConfiguration.Add(new StationConfiguration()
                {
                    Config = config,
                    StationId = stationId,
                    GroupId = null
                }); ;
            }

            // Create new StationConfiguration records for Stations
            foreach (int groupId in req.Groups)
            {
                var dbGroup = await context.Groups.Where(g => groupId == g.GroupId).FirstOrDefaultAsync();
                if (dbGroup == null)
                    return NotFound("Group not found.");

                var stations = await context.StationGroup.Where(sg => sg.GroupId == groupId).ToListAsync();

                context.StationConfiguration.AddRange(stations.Select(s => new StationConfiguration
                {
                    ConfigId = config.ConfigId,
                    StationId = s.StationId,
                    GroupId = s.GroupId
                }));
            }

            await context.SaveChangesAsync();

            return Ok(req);
        }

        // PUT api/<ConfigurationsController>/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ConfigurationDto req)
        {
            // Find the existing configuration record
            Configuration? config = await context.Configurations.FindAsync(id);

            if (config == null)
                return NotFound();

            // Delete existing StationConfiguration records for the configuration
            try
            {
                var existingStationConfigs = await context.StationConfiguration.Where(sc => sc.ConfigId == id).ToListAsync();
                context.StationConfiguration.RemoveRange(existingStationConfigs);
            }
            catch { }



            // Update the configuration
            config.ConfigName = req.ConfigName;
            config.BackupType = req.BackupType;
            config.Retention = req.Retention;
            config.PackageSize = req.PackageSize;
            config.Zip = req.Zip;
            config.Periodic = req.Periodic;
            config.Finished = req.Finished;
            config.PeriodCron = req.PeriodCron;

            // Update the BackupSources records
            var existingSources = await context.BackupSources.Where(bs => bs.ConfigId == config.ConfigId).ToListAsync();
            context.BackupSources.RemoveRange(existingSources);

            foreach (BackupSourceDto source in req.Sources)
            {
                context.BackupSources.Add(new BackupSource
                {
                    Config = config,
                    SourcePath = source.Path
                });
            }

            // Update the BackupDestinations records
            var existingDestinations = await context.BackupDestinations.Where(d => d.ConfigId == config.ConfigId).ToListAsync();
            context.BackupDestinations.RemoveRange(existingDestinations);

            foreach (BackupDestinationDto destination in req.Destinations)
            {
                context.BackupDestinations.Add(new BackupDestination
                {
                    Config = config,
                    DestinationPath = destination.Path,
                    DestinationType = destination.Type,
                });
            }

            // Create new StationConfiguration records for Stations
            foreach (int stationId in req.Stations)
            {
                var dbStation = await context.Stations.Where(s => s.StationId == stationId).FirstOrDefaultAsync();
                if (dbStation == null)
                    return NotFound("Station not found.");

                StationConfiguration record = new StationConfiguration()
                {
                    Config = config,
                    StationId = stationId,
                    GroupId = null
                };

                context.StationConfiguration.Add(record);
            }

            await context.SaveChangesAsync();

            // Create new StationConfiguration records for Groups
            foreach (int groupId in req.Groups)
            {
                var dbGroup = await context.Groups.Where(g => g.GroupId == groupId).FirstOrDefaultAsync();
                if (dbGroup == null)
                    return NotFound("Group not found.");

                var stations = await context.StationGroup.Where(sg => sg.GroupId == groupId && !req.Stations.Contains(sg.StationId)).ToListAsync();

                context.StationConfiguration.AddRange(stations.Select(s => new StationConfiguration
                {
                    Config = config,
                    StationId = s.StationId,
                    GroupId = s.GroupId
                }));
            }

            await context.SaveChangesAsync();

            return Ok(req);
        }

        // DELETE api/<ConfigurationsController>/5
        [Authorize(admin = true)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            // Find the existing configuration record
            Configuration? config = await context.Configurations.FindAsync(id);

            if (config == null)
                return NotFound();

            // Delete existing StationConfiguration records for the configuration
            var existingStationConfigs = await context.StationConfiguration.Where(sc => sc.ConfigId == config.ConfigId).ToListAsync();
            context.StationConfiguration.RemoveRange(existingStationConfigs);

            // Delete existing BackupSources records for the configuration
            var existingSources = await context.BackupSources.Where(s => s.ConfigId == config.ConfigId).ToListAsync();
            context.BackupSources.RemoveRange(existingSources);

            // Delete existing BackupDestinations records for the configuration
            var existingDestinations = await context.BackupDestinations.Where(d => d.ConfigId == config.ConfigId).ToListAsync();
            context.BackupDestinations.RemoveRange(existingDestinations);

            // Delete the configuration record
            context.Configurations.Remove(config);

            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
