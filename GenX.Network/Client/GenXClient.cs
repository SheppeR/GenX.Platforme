using Microsoft.Extensions.Configuration;
using Network;
using Network.Enums;
using Network.Packets;
using Serilog;
using Sylver.HandlerInvoker;

namespace GenX.Network.Client;

public class GenXClient : IGenXClient
{
	private readonly ClientConnectionContainer _container;
	private readonly IHandlerInvoker _handlerInvoker;

	public GenXClient(IHandlerInvoker handlerInvoker, IConfiguration configuration)
	{
		_handlerInvoker = handlerInvoker;

		var _host = configuration.GetValue<string>("ServerInfos:Host");
		var _port = configuration.GetValue<int>("ServerInfos:Port");

		_container = ConnectionFactory.CreateClientConnectionContainer(_host, _port);
		_container.ConnectionLost += OnConnectionLost;
		_container.ConnectionEstablished += OnConnectionEstablished;
	}

	public Task Disconnect(CloseReason reason)
	{
		_container.Shutdown(reason, true);

		return Task.CompletedTask;
	}

	public void Send(RequestPacket request)
	{
		_container.Send(request);
	}

	public async Task<T> SendAndReceive<T>(RequestPacket request) where T : ResponsePacket
	{
		return await _container.SendAsync<T>(request);
	}

	public Task<bool> IsConnected => Task.FromResult(_container.IsAlive_TCP);

	private void OnConnectionEstablished(Connection connection, ConnectionType connectionType)
	{
		Log.Debug($"Client connected to: {connection.IPRemoteEndPoint}	|	Connection Type: {connectionType}");

		//TODO REGISTER PACKET
		//connection.RegisterStaticPacketHandler<REQUESTCLASS>(OnReceive);
	}

	private void OnConnectionLost(Connection connection, ConnectionType connectionType, CloseReason closeReason)
	{
		Log.Debug($"Connection closed from {connection.IPRemoteEndPoint} | Reason : {closeReason}");
	}

	private void OnReceive<T>(T packet, Connection connection)
	{
		try
		{
			_handlerInvoker.Invoke(packet?.GetType(), packet, connection);
		}
		catch (Exception)
		{
			Log.Error($"Unknow Packet {packet?.GetType()} received from server");
		}
	}
}