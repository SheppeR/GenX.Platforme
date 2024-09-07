using System.Windows;
using GenX.Client.ViewModels.Windows;
using GenX.Network.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Sylver.HandlerInvoker;
using Wpf.Ui;

namespace GenX.Client;

public partial class App
{
	private static readonly IHost _host = Host.CreateDefaultBuilder()
		.ConfigureAppConfiguration(config => { _ = config.AddJsonFile("appsettings.json", false, true); })
		.ConfigureServices(
			(context, services) =>
			{
				_ = services.AddSingleton(context.Configuration);
				services.AddHandlers();

				_ = services.AddSingleton<MainWindow>();
				_ = services.AddSingleton<MainWindowViewModel>();

				_ = services.AddSingleton<INavigationService, NavigationService>();
				_ = services.AddSingleton<ISnackbarService, SnackbarService>();
				_ = services.AddSingleton<IContentDialogService, ContentDialogService>();

				_ = services.AddSingleton<IGenXClient, GenXClient>();
			}
		).UseSerilog()
		.Build();

	protected override async void OnStartup(StartupEventArgs e)
	{
		await _host.StartAsync();

		var _window = _host.Services.GetRequiredService<MainWindow>();
		_window.Show();

		base.OnStartup(e);
	}

	protected override async void OnExit(ExitEventArgs e)
	{
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