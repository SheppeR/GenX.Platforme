using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.Messaging;
using GenX.Client.Network;
using GenX.Client.Options;
using GenX.Client.Properties;
using GenX.Client.View;
using GenX.Client.ViewModels;
using GenX.Client.ViewModels.Content;
using GenX.Client.ViewModels.User;
using GenX.Client.ViewModels.Windows;
using GenX.Common.Extensions;
using GenX.Common.Helpers.Controls;
using GenX.Common.Helpers.Localizer;
using GenX.Common.Helpers.Logger;
using H.NotifyIcon;
using iNKORE.UI.WPF.Modern;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Sylver.HandlerInvoker;

namespace GenX.Client;

public partial class App
{
    private static readonly IHost _host = Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration(config => { _ = config.AddJsonFile("appsettings.json", false, true); })
        .ConfigureServices((context, services) =>
            {
                services.AddHandlers();

                _ = services.ConfigureWritable<ServerInfos>(context.Configuration.GetSection("ServerInfos"));

                _ = services.AddSingleton<NotifyIconViewModel>();

                _ = services.AddTransient<MainWindow>();
                _ = services.AddSingleton<MainWindowViewModel>();
                _ = services.AddSingleton<LoginWindow>();
                _ = services.AddSingleton<LoginWindowViewModel>();
                _ = services.AddTransient<FriendsWindow>();
                _ = services.AddSingleton<FriendsWindowViewModel>();

                _ = services.AddSingleton<SettingsThemeContentViewModel>();

                _ = services.AddSingleton<IGenXClient, GenXClient>();
                _ = services.AddSingleton<UserViewModel>();
                _ = services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);
            }
        ).UseSerilog()
        .Build();

    private TaskbarIcon? notifyIcon;

    public App()
    {
        Log.Logger = SerilogUtils.SetupClient();
        LocalizerHelper.ConfigureLocalizer(Settings.Default.Language);
    }

    public static bool IsOffline { get; set; }

    protected override async void OnStartup(StartupEventArgs e)
    {
        try
        {
            ConsoleManager.Show();

            if (e.Args.Length != 0)
            {
                foreach (var arg in e.Args)
                {
                    switch (arg)
                    {
                        case "-offline":
                            IsOffline = true;
                            break;
                        default:
                            IsOffline = false;
                            break;
                    }
                }
            }

            await LoadTheme();

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

            notifyIcon = FindResource("NotifyIcon") as TaskbarIcon;
            notifyIcon?.ForceCreate();

            base.OnStartup(e);
        }
        catch (Exception ex)
        {
            Log.Error("Exception {ExMessage}", ex.Message);
        }
    }

    private Task LoadTheme()
    {
        var theme = Settings.Default.Theme;
        var accent = Settings.Default.Accent;
        if (theme != string.Empty)
        {
            ThemeManager.Current.ApplicationTheme = theme switch
            {
                "Dark" => ApplicationTheme.Dark,
                "Light" => ApplicationTheme.Light,
                _ => ThemeManager.Current.ApplicationTheme
            };
        }

        if (accent != string.Empty)
        {
            ThemeManager.Current.AccentColor = (Color)ColorConverter.ConvertFromString(accent);
        }

        return Task.CompletedTask;
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        try
        {
            await _host.StopAsync(TimeSpan.FromSeconds(5));

            notifyIcon?.Dispose();

            base.OnExit(e);
        }
        catch (Exception ex)
        {
            Log.Error("Exception {ExMessage}", ex.Message);
        }
    }

    public static T GetRequiredService<T>()
        where T : class
    {
        return _host.Services.GetRequiredService<T>();
    }
}