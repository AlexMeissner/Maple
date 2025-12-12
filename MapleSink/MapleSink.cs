using DataTransfer;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using System.Net.Http.Json;

namespace MapleSink;

internal class MapleSink(MapleSinkOptions options) : IBatchedLogEventSink
{
    private readonly HttpClient _httpClient = new();
    private readonly string endpoint = $"{options.Host}/logs";

    public async Task EmitBatchAsync(IEnumerable<LogEvent> batch)
    {
        var payload = batch
            .Where(e => e.Level >= options.LogLevel)
            .Select(e => new LogDto(options.Guid, e.Timestamp, e.Level.ToString(), ConvertProperties(e.Properties)))
            .ToList();
        
        if (payload.Count != 0)
        {
            await _httpClient.PostAsJsonAsync(endpoint, payload);
        }
    }

    public Task OnEmptyBatchAsync() => Task.CompletedTask;

    private static Dictionary<string, object> ConvertProperties(IReadOnlyDictionary<string, LogEventPropertyValue> properties)
    {
        var dictionary = new Dictionary<string, object>();

        foreach (var property in properties)
        {
            if (property.Value is ScalarValue scalar && scalar.Value is { } value)
            {
                dictionary[property.Key] = value;
            }
            else
            {
                dictionary[property.Key] = property.Value.ToString();
            }
        }

        return dictionary;
    }
}
