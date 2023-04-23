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
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {

            var config = await context.Configurations.Where(c => c.ConfigId == id)
                .Select(c => new
                {
                    ConfigId = c.ConfigId,
                    ConfigName = c.ConfigName,
                    BackupType = c.BackupType,
                    Retention = c.Retention,
                    PackageSize = c.PackageSize,
                    PeriodCron = c.PeriodCron,
                    Sources = c.BackupSources
                        .Select(bs => new
                        {
                            Path = bs.SourcePath
                        })
                        .ToList(),
                    Destinations = c.BackupDestinations
                        .Select(bs => new
                        {
                            Path = bs.DestinationPath,
                            Type = bs.DestinationType
                        })
                        .ToList(),
                    Groups = c.StationConfigurations!
                        .Select(sc => new
                        {
                            GroupId = sc.GroupId,
                            GroupName = sc.Group!.GroupName
                        }).ToList(),
                    Stations = c.StationConfigurations!
                       .Select(sc => new
                       {
                           StationId = sc.StationId,
                           StationName = sc.Station.StationName
                       }).ToList()

                }).FirstOrDefaultAsync();

            if (config == null)
                return NotFound();

            return Ok(config);
        }


        // POST api/<ConfigurationsController>
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
                PeriodCron = req.PeriodCron
            };

            context.Configurations.Add(config);

            // Create new BackupSources records
            foreach (BackupSourceDto source in req.Sources)
            {
                context.BackupSources.Add(new BackupSource
                {
                    Config = config,
                    SourcePath = source.SourcePath
                });
            }

            // Create new BackupDestinations records
            foreach (BackupDestinationDto destination in req.Destinations)
            {
                context.BackupDestinations.Add(new BackupDestination
                {
                    Config = config,
                    DestinationPath = destination.DestinationPath,
                    DestinationType = destination.DestinationType,
                });
            }

            await context.SaveChangesAsync();

            // Create new StationConfiguration records for Groups
            foreach (StationDto station in req.Stations)
            {
                var dbStation = await context.Stations.Where(s => s.StationId == station.StationId).FirstOrDefaultAsync();
                if (dbStation == null)
                    return NotFound("Station not found.");

                context.StationConfiguration.Add(new StationConfiguration()
                {
                    Config = config,
                    StationId = station.StationId,
                    GroupId = null
                }); ;
            }

            // Create new StationConfiguration records for Stations
            foreach (GroupDto group in req.Groups)
            {
                var dbGroup = await context.Groups.Where(g => group.GroupId == g.GroupId).FirstOrDefaultAsync();
                if (dbGroup == null)
                    return NotFound("Group not found.");

                var stations = await context.StationGroup.Where(sg => sg.GroupId == group.GroupId).ToListAsync();

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
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ConfigurationDto req)
        {
            // Find the existing configuration record
            Configuration? config = await context.Configurations.FindAsync(id);

            if (config == null)
                return NotFound();

            // Delete existing StationConfiguration records for the configuration
            var existingStationConfigs = await context.StationConfiguration.Where(sc => sc.ConfigId == config.ConfigId).ToListAsync();
            context.StationConfiguration.RemoveRange(existingStationConfigs);

            // Update the configuration
            config.ConfigName = req.ConfigName;
            config.BackupType = req.BackupType;
            config.Retention = req.Retention;
            config.PackageSize = req.PackageSize;
            config.PeriodCron = req.PeriodCron;

            // Update the BackupSources records
            var existingSources = await context.BackupSources.Where(bs => bs.ConfigId == config.ConfigId).ToListAsync();
            context.BackupSources.RemoveRange(existingSources);

            foreach (BackupSourceDto source in req.Sources)
            {
                context.BackupSources.Add(new BackupSource
                {
                    Config = config,
                    SourcePath = source.SourcePath
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
                    DestinationPath = destination.DestinationPath,
                    DestinationType = destination.DestinationType,
                });
            }

            // Create new StationConfiguration records for Stations
            foreach (StationDto station in req.Stations)
            {
                var dbStation = await context.Stations.Where(s => s.StationId == station.StationId).FirstOrDefaultAsync();
                if (dbStation == null)
                    return NotFound("Station not found.");

                context.StationConfiguration.Add(new StationConfiguration()
                {
                    Config = config,
                    StationId = station.StationId,
                    GroupId = null
                }); ;
            }

            // Create new StationConfiguration records for Groups
            foreach (GroupDto group in req.Groups)
            {
                var dbGroup = await context.Groups.Where(g => group.GroupId == g.GroupId).FirstOrDefaultAsync();
                if (dbGroup == null)
                    return NotFound("Group not found.");

                var stations = await context.StationGroup.Where(sg => sg.GroupId == group.GroupId).ToListAsync();

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
