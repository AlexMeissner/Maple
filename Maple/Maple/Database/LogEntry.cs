namespace Maple.Database;

public class LogEntry
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public required string Level { get; set; }
    public required Dictionary<string, object> Payload { get; set; }
}
