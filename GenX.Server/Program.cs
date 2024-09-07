using GenX.Server.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using SeriLogThemesLibrary;

Log.Logger = new LoggerConfiguration()
	.MinimumLevel.Debug()
	.MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
	.Enrich.FromLogContext()
	.WriteTo.Console(
		outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
		theme: SeriLogCustomThemes.Theme1())
	.WriteTo.File(
		"Logs/Server.log",
		rollingInterval: RollingInterval.Day,
		fileSizeLimitBytes: 10 * 1024 * 1024,
		retainedFileCountLimit: 2,
		rollOnFileSizeLimit: true,
		shared: true,
		outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
		flushToDiskInterval: TimeSpan.FromSeconds(1))
	.CreateLogger();

using var host = Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostContext, config) =>
	{
		config.SetBasePath(Environment.CurrentDirectory)
			.AddJsonFile("appsettings.json", false, true);
	})
	.ConfigureServices((context, services) =>
	{
		services.AddOptions();

		services.AddDbContextFactory<ServerContext>();

		services.AddSingleton(context.Configuration);
	})
	.ConfigureLogging(builder => { builder.SetMinimumLevel(LogLevel.Trace); })
	.UseConsoleLifetime().UseSerilog().Build();

await host.RunAsync();