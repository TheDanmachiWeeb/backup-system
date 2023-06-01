using BackupSystem.Models;
using BackupSystem.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
using System.Text.Json.Serialization;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackupSystem.Controllers

{
    [Route("api/[controller]")]
    [ApiController]

    public class StationsController : ControllerBase
    {

        private MyContext context = new MyContext();

        // GET: api/<StationsController>
        [Authorize(admin = true)]
        [HttpGet]
        public async Task<ActionResult<List<Station>>> Get([FromQuery] List<string> include)
        {
            var stations = await context.Stations.Select(s => new
            {
                stationId = s.StationId,
                stationName = s.StationName,
                ipAddress = s.IpAddress,
                macAddress = s.MacAddress,
                active = s.Active,
                status = s.Status,
                groups = include.Contains("Groups") ? s.StationGroups.Select(sg => new GroupDto
                {
                    GroupId = sg.Group.GroupId,
                    GroupName = sg.Group.GroupName
                }).ToList() : null,
                configs = include.Contains("Configs") ? s.StationConfigurations.Select(sc => new ConfigurationDto
                {
                    ConfigId = sc.Config.ConfigId,
                    ConfigName = sc.Config.ConfigName,
                    BackupType = sc.Config.BackupType,
                    Retention = sc.Config.Retention,
                    PackageSize = sc.Config.PackageSize,
                    Zip = sc.Config.Zip,
                    Periodic = sc.Config.Periodic,
                    Finished = sc.Config.Finished,
                    PeriodCron = sc.Config.PeriodCron,
                }).ToList() : null
            }).ToListAsync();

            return Ok(stations);
        }

        // GET api/<StationsController>/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Station>> Get(int id)
        {
            var station = await context.Stations
                .Where(s => s.StationId == id)
                .Select(s => new
                {
                    stationId = s.StationId,
                    stationName = s.StationName,
                    ipAddress = s.IpAddress,
                    macAddress = s.MacAddress,
                    active = s.Active,
                    status = s.Status,
                    groups = s.StationGroups
                        .Select(sg => new GroupDto
                        {
                            GroupId = sg.GroupId,
                            GroupName = sg.Group!.GroupName
                        })
                        .ToList(),
                    configs = s.StationConfigurations
                        .Select(sc => new ConfigurationDto
                        {
                            ConfigId = sc.ConfigId,
                            ConfigName = sc.Config.ConfigName,
                            BackupType = sc.Config.BackupType,
                            Retention = sc.Config.Retention,
                            PackageSize = sc.Config.PackageSize,
                            Zip = sc.Config.Zip,
                            Periodic = sc.Config.Periodic,
                            Finished = sc.Config.Finished,
                            PeriodCron = sc.Config.PeriodCron,
                            Sources = sc.Config.BackupSources
                                .Where(bd => bd.ConfigId == sc.Config.ConfigId)
                                .Select(bd => new BackupSourceDto
                                {
                                    Path = bd.SourcePath,
                                }).ToList(),
                            Destinations = sc.Config.BackupDestinations
                                .Where(bd => bd.ConfigId == sc.Config.ConfigId)
                                .Select(bd => new BackupDestinationDto
                                {
                                    Path = bd.DestinationPath,
                                    Type = bd.DestinationType
                                }).ToList()
                        }).ToList()

                })
                .FirstOrDefaultAsync();

            if (station == null)
                return NotFound();

            return Ok(station);
        }

        //// POST api/<StationsController>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Station>> Post([FromBody] Station req)
        {
            await context.Stations.AddAsync(req);
            await context.SaveChangesAsync();

            return Ok(req);
        }

        // PUT api/<StationsController>/5
        [Authorize(admin = true)]
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
            station.Status = req.Status;

            await context.SaveChangesAsync();

            return Ok(await context.Stations.ToListAsync());
        }

        // PUT api/<StationsController>/5
        [Authorize]
        [HttpPut("online/{id}")]
        public async Task<ActionResult> Put(int id)
        {
            Station? station = await context.Stations.FindAsync(id);

            if (station == null)
                return NotFound();

            station.Active = true;

            await context.SaveChangesAsync();

            // Start a background task to reset the station.Active after 10 minutes
            _ = ResetActiveAfterDelay(station);

            return Ok("The station has been successfully sent to active for 10 minutes ");
        }

        // DELETE api/<StationsController>/5
        [Authorize(admin = true)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Station>>> Delete(int id)
        {
            // Find the existing station record
            Station? station = await context.Stations.FindAsync(id);

            if (station == null)
                return NotFound("Station with this ID not found.");

            // Delete existing StationGroup records for the station
            var existingStationGroups = await context.StationGroup.Where(sg => sg.StationId == station.StationId).ToListAsync();
            context.StationGroup.RemoveRange(existingStationGroups);

            // Delete the station record
            context.Stations.Remove(station);

            await context.SaveChangesAsync();

            return Ok(await context.Stations.ToListAsync());
        }

        private async Task ResetActiveAfterDelay(Station station)
        {
            await Task.Delay(TimeSpan.FromMinutes(1));

            // Retrieve the station from the database again (in case it was modified)
            Station? updatedStation = await context.Stations.FindAsync(station.StationId);

            if (updatedStation != null)
            {
                updatedStation.Active = false;
                await context.SaveChangesAsync();
            }
        }
    }
}
