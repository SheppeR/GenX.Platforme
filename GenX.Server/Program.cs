using GenX.Common.Extensions;
using GenX.Common.Helpers.Logger;
using GenX.Server.Controllers;
using GenX.Server.Database;
using GenX.Server.Network;
using GenX.Server.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Sylver.HandlerInvoker;

namespace GenX.Server;

public static class Program
{
    public static IHost? Host { get; private set; }

    private static async Task Main(string[] args)
    {
        Log.Logger = SerilogUtils.SetupServer();

        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((_, config) =>
            {
                config.SetBasePath(Environment.CurrentDirectory)
                    .AddJsonFile("appsettings.json", false, true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddHandlers();

                services.ConfigureWritable<ServerInfos>(context.Configuration.GetSection("ServerInfos"));

                services.AddDbContext<IAppDBContext, AppDBContext>(options =>
                    options.UseMySql(context.Configuration.GetConnectionString("DefaultConnection"),
                        ServerVersion.AutoDetect(context.Configuration.GetConnectionString("DefaultConnection"))));

                services.AddSingleton<IUserController, UserController>();

                services.AddSingleton<IHostedService, GenXServerService>();
                services.AddSingleton<IGenXServer, GenXServer>();
            }).ConfigureLogging(builder =>
            {
                builder.AddFilter("Microsoft", LogLevel.Warning);
                builder.SetMinimumLevel(LogLevel.Trace);
            }).UseConsoleLifetime().UseSerilog().Build();

        await Host.RunAsync();
    }
}