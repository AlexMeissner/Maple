using Microsoft.EntityFrameworkCore;

namespace Maple.Database;

public class MapleDatabaseContext(DbContextOptions<MapleDatabaseContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LogEntry>()
            .Property(b => b.Payload)
            .HasColumnType("jsonb");
    }
}
