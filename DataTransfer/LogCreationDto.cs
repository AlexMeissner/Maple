namespace DataTransfer;

public record LogCreationDto(Guid Guid, DateTimeOffset Timestamp, string Level, string Message, Dictionary<string, object> Properties);

public record LogEntryDto(Guid Guid, DateTime Timestamp, string Level, string Message, Dictionary<string, object> Properties);
