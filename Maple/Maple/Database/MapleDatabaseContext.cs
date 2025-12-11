using Microsoft.EntityFrameworkCore;

namespace Maple.Database;

public class MapleDatabaseContext(DbContextOptions<MapleDatabaseContext> options) : DbContext(options)
{
}
