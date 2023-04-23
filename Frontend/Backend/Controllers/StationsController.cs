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
        [HttpGet]
        public async Task<ActionResult<List<Station>>> Get([FromQuery] List<string> include)
        {
            var stations = await context.Stations.Select(s => new
            {
                StationId = s.StationId,
                StationName = s.StationName,
                IpAddress = s.IpAddress,
                macAddress = s.MacAddress,
                active = s.Active,
                Groups = include.Contains("Groups") ? s.StationGroups.Select(sg => new GroupDto
                {
                    GroupId = sg.Group.GroupId,
                    GroupName = sg.Group.GroupName
                }).ToList() : null,
                Configs = include.Contains("Configs") ? s.StationConfigurations.Select(sc => new ConfigurationDto
                {
                    ConfigId = sc.Config.ConfigId,
                    ConfigName = sc.Config.ConfigName,
                    BackupType = sc.Config.BackupType,
                    Retention = sc.Config.Retention,
                    PackageSize = sc.Config.PackageSize,
                    PeriodCron = sc.Config.PeriodCron,
                }).ToList() : null
            }).ToListAsync();

            return Ok(stations);
        }

        // GET api/<StationsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Station>> Get(int id)
        {
            var station = await context.Stations
                .Where(s => s.StationId == id)
                .Select(s => new
                {
                    StationId = s.StationId,
                    StationName = s.StationName,
                    IpAddress = s.IpAddress,
                    MacAddress = s.MacAddress,
                    Active = s.Active,
                    Groups = s.StationGroups
                        .Select(sg => new GroupDto
                        {
                            GroupId = sg.GroupId,
                            GroupName = sg.Group!.GroupName
                        })
                        .ToList(),
                    Configs = s.StationConfigurations
                        .Select(sc => new ConfigurationDto
                        {
                            ConfigId = sc.ConfigId,
                            ConfigName = sc.Config.ConfigName,
                            BackupType = sc.Config.BackupType,
                            Retention = sc.Config.Retention,
                            PackageSize = sc.Config.PackageSize,
                            PeriodCron = sc.Config.PeriodCron,
                            BackupSources = sc.Config.BackupSources
                                .Where(bd => bd.ConfigId == sc.Config.ConfigId)
                                .Select(bd => new BackupSourceDto
                                {
                                    SourcePath = bd.SourcePath,
                                }).ToList(),
                            BackupDestinations = sc.Config.BackupDestinations
                                .Where(bd => bd.ConfigId == sc.Config.ConfigId)
                                .Select(bd => new BackupDestinationDto
                                {
                                    DestinationPath = bd.DestinationPath,
                                    DestinationType = bd.DestinationType
                                }).ToList()
                        }).ToList()
                        
                })
                .FirstOrDefaultAsync();

            if (station == null)
                return NotFound();

            return Ok(station);
        }

        //// POST api/<StationsController>
        [HttpPost]
        public async Task<ActionResult<Station>> Post([FromBody] Station req)
        {
            await context.Stations.AddAsync(req);
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
            // Find the existing station record
            Station? station = await context.Stations.FindAsync(id);

            if (station == null)
                return NotFound("Station with this ID not found.");

            // Delete existing StationConfiguration records for the station
            var existingStationConfigs = await context.StationConfiguration.Where(sc => sc.StationId == station.StationId).ToListAsync();
            context.StationConfiguration.RemoveRange(existingStationConfigs);

            // Delete existing StationGroup records for the station
            var existingStationGroups = await context.StationGroup.Where(sg => sg.StationId == station.StationId).ToListAsync();
            context.StationGroup.RemoveRange(existingStationGroups);

            // Delete the station record
            context.Stations.Remove(station);

            await context.SaveChangesAsync();

            return Ok(await context.Stations.ToListAsync());
        }
    }
}
