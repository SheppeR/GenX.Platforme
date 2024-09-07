using Serilog;
using Serilog.Events;
using SeriLogThemesLibrary;

namespace GenX.Client.Utils;

internal class LoggerHelper
{
	public static void ConfigureLogger()
	{
		Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Debug()
			.MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
			.Enrich.FromLogContext()
			.WriteTo.Console(
				outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
				theme: SeriLogCustomThemes.Theme1())
			.WriteTo.File(
				"Logs/Clients.log",
				rollingInterval: RollingInterval.Day,
				fileSizeLimitBytes: 10 * 1024 * 1024,
				retainedFileCountLimit: 2,
				rollOnFileSizeLimit: true,
				shared: true,
				outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
				flushToDiskInterval: TimeSpan.FromSeconds(1))
			.CreateLogger();
	}
}