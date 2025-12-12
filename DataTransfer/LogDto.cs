namespace DataTransfer;

public record LogDto(Guid Guid, DateTimeOffset TimeStamp, string Level, Dictionary<string, object> Properties);
