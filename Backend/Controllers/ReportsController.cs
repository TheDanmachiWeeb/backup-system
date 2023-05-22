using BackupSystem.Dtos;
using BackupSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackupSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {

        private MyContext context = new MyContext();

        // GET: api/<ReportsController>
        [HttpGet]
        public async Task<ActionResult<List<Report>>> Get()
        {
            //var reports = await context.Reports.ToListAsync();
            //return Ok(reports);

            var reports = await context.Reports
                .Include(r => r.Config)
                .Include(r => r.Station)
                .ToListAsync();

            var result = reports.Select(r => new
            {
                ReportId = r.ReportId,
                StationId = r.StationId,
                StationName = r.Station.StationName,
                ConfigId = r.ConfigId,
                ConfigName = r.Config.ConfigName,
                ReportTime = r.ReportTime,
                BackupSize = r.BackupSize,
                Success = r.Success
            });

            return Ok(result);
        }

        // GET api/<ReportsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Report>> Get(int id)
        {
            var report = await context.Reports.FindAsync(id);

            if (report == null)
            {
                return NotFound();
            }

            return report;
        }

        // POST api/<ReportsController>
        [HttpPost]
        public async Task<ActionResult<Report>> Post([FromBody] ReportDto req)
        {
            Report report = new Report
            {
                StationId = req.StationId,
                ConfigId = req.ConfigId,
                ReportTime = req.ReportTime,
                BackupSize = req.BackupSize,
                Success = req.Success
            };
            context.Reports.Add(report);
            await context.SaveChangesAsync();

            return Ok(report);
        }

        // PUT api/<ReportsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Report>> Put(int id, [FromBody] Report req)
        {
            Report? dbReport = await context.Reports.FindAsync(req);
            if (dbReport == null)
                return NotFound("Report not found.");

            context.Reports.Add(req);
            await context.SaveChangesAsync();

            return Ok(req);
        }

        // DELETE api/<ReportsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Report>>> Delete(int id)
        {
            Report? dbReport = await context.Reports.FindAsync(id);
            if (dbReport == null)
                return NotFound("Report not found.");

            context.Reports.Remove(dbReport);
            await context.SaveChangesAsync();

            return Ok(await context.Reports.ToListAsync());
        }
    }
}
