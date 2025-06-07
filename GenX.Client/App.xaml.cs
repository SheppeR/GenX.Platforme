using System.Windows;
using GenX.Client.Network;
using GenX.Client.Options;
using GenX.Client.Properties;
using GenX.Client.View;
using GenX.Client.ViewModels.Windows;
using GenX.Common.Extensions;
using GenX.Common.Helpers.Localizer;
using GenX.Common.Helpers.Logger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Network.Enums;
using Serilog;
using Sylver.HandlerInvoker;

namespace GenX.Client;

public partial class App
{
    private static readonly IHost _host = Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration(config => { _ = config.AddJsonFile("appsettings.json", false, true); })
        .ConfigureServices(
            (context, services) =>
            {
                services.AddHandlers();

                _ = services.ConfigureWritable<ServerInfos>(context.Configuration.GetSection("ServerInfos"));

                _ = services.AddSingleton<MainWindow>();
                _ = services.AddSingleton<MainWindowViewModel>();
                _ = services.AddSingleton<LoginWindow>();
                _ = services.AddSingleton<LoginWindowViewModel>();
                _ = services.AddSingleton<LoadingWindow>();
                _ = services.AddSingleton<LoadingWindowViewModel>();
                /*_ = services.AddSingleton<INavigationService, NavigationService>();
                _ = services.AddSingleton<ISnackbarService, SnackbarService>();
                _ = services.AddSingleton<IContentDialogService, ContentDialogService>();*/

                _ = services.AddSingleton<IGenXClient, GenXClient>();
            }
        ).UseSerilog()
        .Build();

    public App()
    {
        Log.Logger = SerilogUtils.SetupClient();
        LocalizerHelper.ConfigureLocalizer(Settings.Default.Language);
    }

    public static bool IsOffline { get; set; }

    protected override async void OnStartup(StartupEventArgs e)
    {
        if (e.Args.Length != 0)
            foreach (var arg in e.Args)
                switch (arg)
                {
                    case "-offline":
                        IsOffline = true;
                        break;
                    default:
                        IsOffline = false;
                        break;
                }

        await _host.StartAsync();

        if (IsOffline)
        {
            var _window = _host.Services.GetRequiredService<MainWindow>();
            _window.Show();
        }
        else
        {
            var isConnected = await _host.Services.GetRequiredService<IGenXClient>().IsConnected;
            //TODO HANDLE SERVER DOWN
            while (!isConnected)
            {
                isConnected = await _host.Services.GetRequiredService<IGenXClient>().IsConnected;
                await Task.Delay(1);
            }

            var _window = _host.Services.GetRequiredService<LoginWindow>();
            _window.Show();
        }

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.Services.GetRequiredService<IGenXClient>().Disconnect(CloseReason.ClientClosed);
        await _host.StopAsync(TimeSpan.FromSeconds(5));
        _host.Dispose();

        base.OnExit(e);
    }

    public static T GetRequiredService<T>()
        where T : class
    {
        return _host.Services.GetRequiredService<T>();
    }
}