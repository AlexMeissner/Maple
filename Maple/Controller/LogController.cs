using DataTransfer;
using Maple.Database;
using Microsoft.AspNetCore.Mvc;

namespace Maple.Controller;

[ApiController]
public class LogController(MapleDatabaseContext dbContext) : ControllerBase
{
    private readonly MapleDatabaseContext _dbContext = dbContext;

    [HttpPost("/logs")]
    public async Task<IActionResult> PostLogs(List<LogDto> logs)
    {
        if (logs.Count == 0)
        {
            return BadRequest("No log messages provided");
        }

        var entry = logs.Select(l => new LogEntry
        {
            Timestamp = DateTime.UtcNow,
            Level = l.Level,
            Properties = l.Properties
        }).ToList();

        await _dbContext.AddRangeAsync(entry);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}
