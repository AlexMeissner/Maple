using MapleSink;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Maple(configuration)
    .WriteTo.Console()
    .CreateLogger();

Log.Logger.Verbose("This is a {logLevel} log message. {number}", LogEventLevel.Verbose, 12);
Log.Logger.Information("This is an {logLevel} log message. {string}", LogEventLevel.Information, "Peter Pan");
Log.Logger.Warning("This is a {logLevel} log message. {timestamp}", LogEventLevel.Warning, DateTime.UtcNow);
Log.Logger.Error("This is an {logLevel} log message. {number} {string} {timestamp}", LogEventLevel.Error, 24, "LoL", DateTime.Now);

Log.CloseAndFlush();

await Task.Delay(2000);
