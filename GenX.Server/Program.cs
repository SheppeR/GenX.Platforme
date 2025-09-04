using GenX.Common.Extensions;
using GenX.Common.Helpers.Logger;
using GenX.Server.Controllers.Friends;
using GenX.Server.Controllers.Users;
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
    private static IHost? Host { get; set; }

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

                services.AddScoped<IUserController, UserController>();
                services.AddScoped<IFriendController, FriendController>();

                services.AddScoped<IHostedService, GenXServerService>();
                services.AddScoped<IGenXServer, GenXServer>();
            }).ConfigureLogging(builder =>
            {
                builder.AddFilter("Microsoft", LogLevel.Warning);
                builder.SetMinimumLevel(LogLevel.Trace);
            }).UseConsoleLifetime().UseSerilog().Build();

        await Host.RunAsync();
    }
}