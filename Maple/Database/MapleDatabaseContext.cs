using Microsoft.EntityFrameworkCore;

namespace Maple.Database;

internal class MapleDatabaseContext(DbContextOptions<MapleDatabaseContext> options) : DbContext(options)
{
    public DbSet<LogEntry> LogEntries { get; set; }
    public DbSet<Project> Projects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LogEntry>()
            .Property(b => b.Properties)
            .HasColumnType("jsonb");
    }

    // ToDo. Understand Indices
    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<MyEntity>()
    //        .HasIndex(e => e.Guid)
    //        .IsUnique(false); // It should not be unique, as you expect duplicates
    //}
}
