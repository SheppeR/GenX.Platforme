using Microsoft.Extensions.Hosting;
using Serilog;

namespace GenX.Network.Server;

public class GenXHostedServer : IHostedService
{
	private readonly IGenXServer _server;

	public GenXHostedServer(IGenXServer server, IServiceProvider serviceProvider)
	{
		_server = server;
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		Log.Information("Starting Server");
		await _server.Start();
	}

	public async Task StopAsync(CancellationToken cancellationToken)
	{
		Log.Information("Stoping Server");
		await _server.Stop();
	}
}