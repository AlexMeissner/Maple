using MapleSink;
using Microsoft.Extensions.Configuration;
using Serilog;

var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Maple(configuration)
    .WriteTo.Console()
    .CreateLogger();

Log.Logger.Verbose("This is a verbose log message.");
Log.Logger.Information("This is an information log message.");
Log.Logger.Warning("This is a warning log message.");
Log.Logger.Error("This is an error log message.");

Log.CloseAndFlush();

await Task.Delay(2000);
