//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using BackupSystem.Models;
//using Newtonsoft.Json;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace BackupSystem.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class GroupsController : ControllerBase
//    {
//        private MyContext context = new MyContext();

//        // GET: api/<GroupsController>
//        [HttpGet]
//        public async Task<ActionResult> Get()
//        {
//            var groups = await context.Groups
//                .Select(g => new 
//                {
//                    GroupId = g.GroupId,
//                    GroupName = g.GroupName,
//                    Stations = context.StationGroup
//                        .Where(sg => sg.GroupId == g.GroupId)
//                        .Select(s => s.StationId)
//                        .ToList()
//                })
//                .ToListAsync();

//            return Ok(groups);
//        }

//        // GET api/<GroupsController>/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Group>> Get(int id)
//        {
//            var group = await context.Groups.Where(g => g.GroupId == id)
//                .Select(g => new
//                {
//                    GroupId = g.GroupId,
//                    GroupName = g.GroupName,
//                    Stations = g.StationGroups.Select(sg => sg.StationId).ToList()
//                }).ToListAsync();

//            if (group == null)
//                return NotFound();

//            return Ok(group);
//        }

//        // POST api/<GroupsController>
//        [HttpPost]
//        public async Task<ActionResult<Group>> Post(Group req)
//        {

//            // Add the group to the database
//            Group group = new Group
//            {
//                GroupName = req.GroupName,
//            };

//            context.Groups.Add(group);

//            await context.SaveChangesAsync();

//            // Add the stations to the StationGroup table
//            foreach (Station station in req.Stations)
//            {
//                context.StationGroup.Add(new StationGroup { StationId = station.StationId, GroupId = group.GroupId });
//            }

//            await context.SaveChangesAsync();

//            // Serialize the group object to JSON with ReferenceLoopHandling.Ignore setting
//            var settings = new JsonSerializerSettings
//            {
//                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
//            };
//            string json = JsonConvert.SerializeObject(group, settings);

//            // Return the created group with the added stations as JSON
//            return Content(json, "application/json");
//        }

//        // PUT api/<GroupsController>/5
//        [HttpPut("{id}")]
//        public async Task<ActionResult<Group>> Put(int id, Group req)
//        {
//            // Find the group to update
//            Group? group = await context.Groups.FindAsync(id);

//            if (group == null)
//                return NotFound("Group not found.");

//            // Update the group name
//            group.GroupName = req.GroupName;

//            // Remove all StationGroup records with the updated group ID
//            context.StationGroup.RemoveRange(context.StationGroup.Where(sg => sg.GroupId == id));


//            // Add the updated stations to the StationGroup table
//            foreach (Station station in req.Stations)
//            {
//                context.StationGroup.Add(new StationGroup { StationId = station.StationId, GroupId = id });
//            }

//            var stationIds = req.Stations.Select(s => s.StationId).ToList();

//            // Get all configs attached to this group
//            var configs = await context.StationConfiguration.Where(c => c.GroupId == id).Select(c => c.ConfigId).ToListAsync();

//            // Remove all StationConfiguration records with the updated group ID
//            context.StationConfiguration.RemoveRange(context.StationConfiguration.Where(sc => sc.GroupId == id));

//            await context.SaveChangesAsync();

//            // Set the group
//            if (configs != null && stationIds != null)
//            {
//                foreach (int configId in configs)
//                {
//                    foreach (int stationId in stationIds)
//                    {
//                        context.StationConfiguration.Add(new StationConfiguration { ConfigId = configId, StationId = stationId, GroupId = id });
//                    }
//                }
//            }

//            await context.SaveChangesAsync();

//            var settings = new JsonSerializerSettings
//            {
//                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
//            };
//            string json = JsonConvert.SerializeObject(group, settings);

//            return Content(json, "application/json");
//        }

//        // DELETE api/<GroupsController>/5
//        [HttpDelete("{id}")]
//        public async Task<ActionResult> Delete(int id)
//        {
//            // Find the group to delete
//            Group? group = await context.Groups.FindAsync(id);

//            if (group == null)
//                return NotFound("Group not found.");

//            // Remove all StationGroup records associated with the group
//            context.StationGroup.RemoveRange(context.StationGroup.Where(sg => sg.GroupId == id));

//            // Remove all StationConfiguration records associated with the group
//            context.StationConfiguration.RemoveRange(context.StationConfiguration.Where(sc => sc.GroupId == id));

//            // Remove the group
//            context.Groups.Remove(group);

//            await context.SaveChangesAsync();

//            return Ok();
//        }
//    }
//}
