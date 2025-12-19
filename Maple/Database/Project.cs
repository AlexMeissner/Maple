using System.ComponentModel.DataAnnotations;

namespace Maple.Database;

internal class Project
{
    [Key]
    public Guid Guid { get; set; }
    public required string Name { get; set; }
}
