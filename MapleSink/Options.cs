using Serilog.Events;

namespace MapleSink;

internal class BatchingOptions
{
    public bool? EagerlyEmitFirstEvent { get; set; }
    public int? BatchSizeLimit { get; set; }
    public int? Period { get; set; }
    public int? QueueLimit { get; set; }
}

internal class MapleSinkOptions
{
    public Guid Guid { get; set; }
    public required string Host { get; set; }
    public LogEventLevel? LogLevel { get; set; }
    public BatchingOptions? Batching { get; set; }

}