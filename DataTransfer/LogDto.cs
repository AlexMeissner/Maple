namespace DataTransfer;

public record LogDto(string Level, Dictionary<string, object> Properties);
