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
            .Select(e => new LogDto(options.Guid, e.Timestamp, e.Level.ToString(), (Dictionary<string, object>)e.Properties));

        if (payload.Any())
        {
            await _httpClient.PostAsJsonAsync(endpoint, payload);
        }
    }

    public Task OnEmptyBatchAsync() => Task.CompletedTask;
}
