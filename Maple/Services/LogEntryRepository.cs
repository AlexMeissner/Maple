using DataTransfer;
using Maple.Database;
using Microsoft.EntityFrameworkCore;

namespace Maple.Services;

internal interface ILogEntryRepository
{
    IEnumerable<LogEntryDto> Get(string? level, string? filter);
    IEnumerable<Guid> GetProjects();
}

internal class LogEntryRepository(MapleDatabaseContext dbContext) : ILogEntryRepository
{
    private readonly MapleDatabaseContext _dbContext = dbContext;

    public IEnumerable<LogEntryDto> Get(string? level, string? filter)
    {
        var query = _dbContext.LogEntries.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(level))
        {
            query = query.Where(x => x.Level == level);
        }

        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(l => l.Properties != null && EF.Functions.ILike(l.Properties.ToString() ?? "", $"%{filter}"));
        }

        var logs = query
            .OrderByDescending(l => l.Timestamp)
            .Take(100)
            .Select(l => new LogEntryDto(l.Guid, l.Timestamp, l.Level, l.Message, l.Properties));

        return logs;
    }

    public IEnumerable<Guid> GetProjects()
    {
        return _dbContext.LogEntries
             .AsNoTracking()
             .Select(l => l.Guid)
             .Distinct();
    }
}
