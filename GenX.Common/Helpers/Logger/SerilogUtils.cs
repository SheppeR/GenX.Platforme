using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace GenX.Common.Helpers.Logger;

public class SerilogUtils
{
    public static Serilog.Core.Logger SetupClient()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Error)
            .Enrich.FromLogContext()
            .WriteTo.File(
                "Logs/Clients.log",
                rollingInterval: RollingInterval.Day,
                fileSizeLimitBytes: 10 * 1024 * 1024,
                retainedFileCountLimit: 2,
                rollOnFileSizeLimit: true,
                shared: true,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                flushToDiskInterval: TimeSpan.FromSeconds(1))
            .CreateLogger();
    }

    public static Serilog.Core.Logger SetupServer()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Fatal) // ou Error
            .MinimumLevel.Override("Pomelo.EntityFrameworkCore.MySql", LogEventLevel.Fatal)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Fatal)
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                theme: Theme())
            .WriteTo.File(
                "Logs/Server.log",
                rollingInterval: RollingInterval.Day,
                fileSizeLimitBytes: 10 * 1024 * 1024,
                retainedFileCountLimit: 2,
                rollOnFileSizeLimit: true,
                shared: true,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                flushToDiskInterval: TimeSpan.FromSeconds(1))
            .CreateLogger();
    }

    public static SystemConsoleTheme Theme()
    {
        Dictionary<ConsoleThemeStyle, SystemConsoleThemeStyle> customThemeStyles = new()
        {
            {
                ConsoleThemeStyle.Text, new SystemConsoleThemeStyle
                {
                    Foreground = ConsoleColor.Green
                }
            },
            {
                ConsoleThemeStyle.String, new SystemConsoleThemeStyle
                {
                    Foreground = ConsoleColor.Yellow
                }
            },
            {
                ConsoleThemeStyle.Number, new SystemConsoleThemeStyle
                {
                    Foreground = ConsoleColor.Cyan
                }
            },
            {
                ConsoleThemeStyle.Boolean, new SystemConsoleThemeStyle
                {
                    Foreground = ConsoleColor.Red
                }
            },
            {
                ConsoleThemeStyle.LevelInformation, new SystemConsoleThemeStyle
                {
                    Foreground = ConsoleColor.DarkCyan
                }
            },
            {
                ConsoleThemeStyle.SecondaryText, new SystemConsoleThemeStyle
                {
                    Foreground = ConsoleColor.White
                }
            },
            {
                ConsoleThemeStyle.Null, new SystemConsoleThemeStyle
                {
                    Foreground = ConsoleColor.Black,
                    Background = ConsoleColor.Yellow
                }
            },
            {
                ConsoleThemeStyle.LevelError, new SystemConsoleThemeStyle
                {
                    Foreground = ConsoleColor.White,
                    Background = ConsoleColor.Red
                }
            },
            {
                ConsoleThemeStyle.LevelFatal, new SystemConsoleThemeStyle
                {
                    Foreground = ConsoleColor.Black, Background = ConsoleColor.Cyan
                }
            },
            {
                ConsoleThemeStyle.LevelWarning, new SystemConsoleThemeStyle
                {
                    Foreground = ConsoleColor.Magenta,
                    Background = ConsoleColor.Yellow
                }
            },
            {
                ConsoleThemeStyle.LevelVerbose, new SystemConsoleThemeStyle
                {
                    Foreground = ConsoleColor.DarkMagenta
                }
            },
            {
                ConsoleThemeStyle.Name, new SystemConsoleThemeStyle
                {
                    Foreground = ConsoleColor.Black,
                    Background = ConsoleColor.Yellow
                }
            },
            {
                ConsoleThemeStyle.Scalar, new SystemConsoleThemeStyle
                {
                    Foreground = ConsoleColor.White
                }
            }
        };

        return new SystemConsoleTheme(customThemeStyles);
    }

    public static void PrintSection(string section)
    {
        section = "-[ " + section + " ]";

        while (section.Length < 79) section = "=" + section;

        Log.Verbose($"{section}");
    }
}