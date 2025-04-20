using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Exceptions;
using FileOptions = RookiEcom.Infrastructure.Logging.FileOptions;

namespace Microsoft.Extensions.DependencyInjection;

public static class Extension
{
    private static void UseLogger(this IWebHostEnvironment env, LoggingOptions options)
    {
        var assemblyName = Assembly.GetEntryAssembly()?.GetName().Name;

        var logsPath = Path.Combine(env.ContentRootPath, "logs");
        Directory.CreateDirectory(logsPath);
        var loggerConfiguration = new LoggerConfiguration();

        loggerConfiguration = loggerConfiguration
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentUserName()
            .Enrich.WithProperty("ProcessId", Environment.ProcessId)
            .Enrich.WithProperty("Assembly", assemblyName)
            .Enrich.WithProperty("Application", env.ApplicationName)
            .Enrich.WithProperty("EnvironmentName", env.EnvironmentName)
            .Enrich.WithProperty("ContentRootPath", env.ContentRootPath)
            .Enrich.WithProperty("WebRootPath", env.WebRootPath)
            .Enrich.WithExceptionDetails()
            .WriteTo.File(Path.Combine(logsPath, "log.txt"),
                fileSizeLimitBytes: 10 * 1024 * 1024,
                rollOnFileSizeLimit: true,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(1),
                outputTemplate:
                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] [TraceId: {TraceId}] [MachineName: {MachineName}] [ProcessId: {ProcessId}] {Message:lj}{NewLine}{Exception}",
                restrictedToMinimumLevel: options.File.MinimumLogEventLevel)
            .WriteTo.Console(
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}");
        Log.Logger = loggerConfiguration.CreateLogger();
    }

    private static LoggingOptions SetDefault(LoggingOptions options)
    {
        options ??= new LoggingOptions
        {
        };

        options.LogLevel ??= new Dictionary<string, string>();

        if (!options.LogLevel.ContainsKey("Default"))
        {
            options.LogLevel["Default"] = "Warning";
        }

        options.File ??= new FileOptions
        {
            MinimumLogEventLevel = Serilog.Events.LogEventLevel.Warning,
        };

        return options;
    }

    public static IWebHostBuilder UseLogger(this IWebHostBuilder builder,
        Func<IConfiguration, LoggingOptions> logOptions)
    {
        builder.ConfigureLogging((context, logging) =>
        {
            logging.Configure(options => { });

            logging.AddSerilog();

            LoggingOptions options = SetDefault(logOptions(context.Configuration));

            context.HostingEnvironment.UseLogger(options);
        });

        return builder;
    }
}
