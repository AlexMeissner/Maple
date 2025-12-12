using Microsoft.EntityFrameworkCore;

namespace Maple.Database;

public class MapleDatabaseContext(DbContextOptions<MapleDatabaseContext> options) : DbContext(options)
{
    public DbSet<LogEntry> LogEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LogEntry>()
            .Property(b => b.Properties)
            .HasColumnType("jsonb");
    }
}
