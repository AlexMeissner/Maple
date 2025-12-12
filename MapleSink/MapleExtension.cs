using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Configuration;
using Serilog.Sinks.PeriodicBatching;

namespace MapleSink;

public static class MapleExtension
{
    public static LoggerConfiguration Maple(
        this LoggerSinkConfiguration sinkConfiguration,
        IConfiguration configuration)
    {
        var mapleOptions = configuration.GetRequiredSection("Maple").Get<MapleSinkOptions>()
            ?? throw new Exception("Maple section not found");

        var sinkOptions = new PeriodicBatchingSinkOptions();

        if (mapleOptions.Batching is { } batchingOptions)
        {
            if (batchingOptions.EagerlyEmitFirstEvent is { } eagerlyEmitFirstEvent)
            {
                sinkOptions.EagerlyEmitFirstEvent = eagerlyEmitFirstEvent;
            }
            if (batchingOptions.BatchSizeLimit is { } batchSizeLimit)
            {
                sinkOptions.BatchSizeLimit = batchSizeLimit;
            }
            if (batchingOptions.Period is { } period)
            {
                sinkOptions.Period = TimeSpan.FromSeconds(period);
            }
            if (batchingOptions.QueueLimit is { } queueLimit)
            {
                sinkOptions.QueueLimit = queueLimit;
            }
        }

        var batchedSink = new PeriodicBatchingSink(new MapleSink(mapleOptions), sinkOptions);

        return sinkConfiguration.Sink(batchedSink);
    }
}
