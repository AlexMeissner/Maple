using DataTransfer;
using Maple.Database;
using Microsoft.AspNetCore.Mvc;

namespace Maple.Controller;

[ApiController]
[Route("/log-entires")]
internal class LogController(MapleDatabaseContext dbContext) : ControllerBase
{
    private readonly MapleDatabaseContext _dbContext = dbContext;

    [HttpPost]
    public async Task<IActionResult> PostLogs(List<LogCreationDto> logs)
    {
        if (logs.Count == 0)
        {
            return BadRequest("No log messages provided");
        }

        var entry = logs.Select(l => new LogEntry
        {
            Guid = l.Guid,
            Timestamp = l.Timestamp.UtcDateTime,
            Level = l.Level,
            Message = l.Message,
            Properties = l.Properties
        }).ToList();

        await _dbContext.AddRangeAsync(entry);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}
