using Microsoft.AspNetCore.Mvc;
using BackupSystem.Dtos;
using Microsoft.EntityFrameworkCore;
using BackupSystem.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Ocsp;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackupSystem.Controllers
{
    [Authorize(admin = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private MyContext context = new MyContext();

        // GET: api/<GroupsController>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var groups = await context.Groups
                .Select(g => new
                {
                    GroupId = g.GroupId,
                    GroupName = g.GroupName,
                    Stations = context.StationGroup
                            .Where(sg => sg.GroupId == g.GroupId)
                        .Select(s => s.StationId)
                        .ToList()
                })
                .ToListAsync();

            return Ok(groups);
        }

        // GET api/<GroupsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> Get(int id)
        {
            var group = await context.Groups
            .Include(g => g.StationGroups)
                .ThenInclude(sc => sc.Station)
            .Include(g => g.StationConfigurations)
                .ThenInclude(sc => sc.Config)
            .Include(g => g.StationConfigurations)
                .ThenInclude(sc => sc.Station)
            .FirstOrDefaultAsync(g => g.GroupId == id);

            if (group == null)
                return NotFound();

            var result = new
            {
                GroupId = group.GroupId,
                GroupName = group.GroupName,
                Stations = group.StationGroups
                    .Select(sc => new
                    {
                        StationId = sc.Station.StationId,
                        StationName = sc.Station.StationName
                    })

                    .ToList(),
                Configs = group.StationConfigurations
                    .Select(sc => new
                    {
                        ConfigId = sc.Config.ConfigId,
                        ConfigName = sc.Config.ConfigName
                    })
                    .Distinct()
                    .ToList(),
            };

            return Ok(result);
        }

        //POST api/<GroupsController>
        [HttpPost]
        public async Task<ActionResult<Group>> Post(GroupDto req)
        {

            // Add the group to the database
            Group group = new Group
            {
                GroupName = req.GroupName,
            };

            context.Groups.Add(group);

            await context.SaveChangesAsync();

            foreach (var config in req.Configs)
            {
                StationConfiguration? record = await context.StationConfiguration.FirstOrDefaultAsync(sc => sc.ConfigId == config && req.Stations.Contains(sc.StationId));
                if (record != null)
                    context.StationConfiguration.Remove(record);
            }

            await context.SaveChangesAsync();


            // Add the stations to the StationGroup table
            foreach (int stationId in req.Stations)
            {
                context.StationGroup.Add(new StationGroup { StationId = stationId, GroupId = group.GroupId });
                foreach (int configId in req.Configs)
                {
                    context.StationConfiguration.Add(new StationConfiguration { ConfigId = configId, StationId = stationId, GroupId = group.GroupId });
                }
            }

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            string json = JsonConvert.SerializeObject(group, settings);

            return Content(json, "application/json");

        }

        // PUT api/<GroupsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Group>> Put(int id, [FromBody] GroupDto req)
        {
            // Find the group to update
            Group? group = await context.Groups.FindAsync(id);

            if (group == null)
                return NotFound("Group not found.");

            // Update the group name
            group.GroupName = req.GroupName;

            // Remove all StationGroup records with the updated group ID
            context.StationGroup.RemoveRange(context.StationGroup.Where(sg => sg.GroupId == id));


            // Add the updated stations to the StationGroup table
            foreach (int stationId in req.Stations)
            {
                context.StationGroup.Add(new StationGroup { StationId = stationId, GroupId = id });
            }

            // Remove all StationConfiguration records with the updated group ID
            context.StationConfiguration.RemoveRange(context.StationConfiguration.Where(sc => sc.GroupId == id || req.Stations.Contains(sc.StationId)));

            await context.SaveChangesAsync();

            // Set the group
            if (req.Configs != null && req.Stations != null)
            {
                foreach (int configId in req.Configs)
                {
                    foreach (int stationId in req.Stations)
                    {

                        context.StationConfiguration.Add(new StationConfiguration { ConfigId = configId, StationId = stationId, GroupId = id });
                    }
                }
            }

            await context.SaveChangesAsync();

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            string json = JsonConvert.SerializeObject(group, settings);

            return Content(json, "application/json");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Group>> Delete(int id)
        {
            // Find the group to delete
            Group? group = await context.Groups.Where(g => g.GroupId == id).FirstOrDefaultAsync();

            if (group == null)
                return NotFound("Group not found.");

            context.StationConfiguration.RemoveRange(context.StationConfiguration.Where(sc => sc.GroupId == id));

            // Remove the group
            context.Groups.Remove(group);

            await context.SaveChangesAsync();

            return Ok();


        }
    }
}
