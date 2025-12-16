namespace Maple.Database;

public class LogEntry
{
    public int Id { get; set; }
    public Guid Guid { get; set; }
    public DateTime Timestamp { get; set; }
    public required string Level { get; set; }
    public required string Message { get; set; }
    public required Dictionary<string, object> Properties { get; set; }
}
