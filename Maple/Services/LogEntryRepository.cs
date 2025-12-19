using DataTransfer;
using Maple.Database;
using Microsoft.EntityFrameworkCore;

namespace Maple.Services;

internal interface ILogEntryRepository
{
    IEnumerable<LogEntryDto> Get(Guid projectGuid, string? level, string? filter);
    IEnumerable<Project> GetProjects();
    IEnumerable<Project> GetProjectsFromLogEntries();
    Task UpdateProjectAliases(IEnumerable<Project> projects);
}

internal class LogEntryRepository(MapleDatabaseContext dbContext) : ILogEntryRepository
{
    private readonly MapleDatabaseContext _dbContext = dbContext;

    public IEnumerable<LogEntryDto> Get(Guid projectGuid, string? level, string? filter)
    {
        var query = _dbContext.LogEntries.AsNoTracking();

        if (projectGuid != Guid.Empty)
        {
            query = query.Where(x => x.Guid == projectGuid);
        }

        if (!string.IsNullOrWhiteSpace(level))
        {
            query = query.Where(x => x.Level == level);
        }

        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(l =>
               EF.Functions.ILike(l.Message, $"%{filter}%") ||
               EF.Functions.ILike(l.Properties.ToString()!, $"%{filter}%"));
        }

        var logs = query
            .OrderByDescending(l => l.Timestamp)
            .Take(100)
            .Select(l => new LogEntryDto(l.Guid, l.Timestamp, l.Level, l.Message, l.Properties));

        return logs;
    }

    public IEnumerable<Project> GetProjects()
    {
        var namedProjects = _dbContext.Projects.AsNoTracking().AsEnumerable();

        var unnamedProjects = _dbContext.LogEntries
            .AsNoTracking()
            .Select(l => new Project() { Guid = l.Guid, Name = "" })
            .Distinct()
            .Where(p => namedProjects.Any(x => x.Guid == p.Guid) == false)
            .AsEnumerable();

        return namedProjects.Concat(unnamedProjects);
    }

    public IEnumerable<Project> GetProjectsFromLogEntries()
    {
        return _dbContext.LogEntries
            .AsNoTracking()
            .Select(l => l.Guid)
            .Distinct()
            .GroupJoin(
                _dbContext.Projects.AsNoTracking(),
                logGuid => logGuid, // key from left table, which are the distainct Guids
                project => project.Guid, // key from the right table which should match the other key
                (logGuid, projects) => new Project
                {
                    Guid = logGuid,
                    Name = projects.Select(p => p.Name).FirstOrDefault() ?? logGuid.ToString()
                });
    }

    public async Task UpdateProjectAliases(IEnumerable<Project> projects)
    {
        foreach (var project in projects)
        {
            if (string.IsNullOrWhiteSpace(project.Name)) continue;

            if (await _dbContext.Projects.FindAsync(project.Guid) is { } dbProject)
            {
                dbProject.Name = project.Name;
            }
            else
            {
                await _dbContext.AddAsync(project);
            }
        }

        await _dbContext.SaveChangesAsync();
    }
}
