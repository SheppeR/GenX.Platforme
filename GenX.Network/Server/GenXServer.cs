using Microsoft.Extensions.Configuration;
using Network;
using Network.Enums;
using Serilog;
using Sylver.HandlerInvoker;

namespace GenX.Network.Server;

public class GenXServer : IGenXServer
{
	private readonly ServerConnectionContainer _container;
	private readonly IHandlerInvoker _handlerInvoker;

	public GenXServer(IHandlerInvoker handlerInvoker, IConfiguration configuration, IServiceProvider serviceProvider)
	{
		_handlerInvoker = handlerInvoker;

		var _port = configuration.GetValue<int>("ServerInfos:Port");

		_container = ConnectionFactory.CreateServerConnectionContainer(_port, false);
		_container.ConnectionLost += ConnectionLost;
		_container.ConnectionEstablished += ConnectionEstablished;
		_container.AllowUDPConnections = true;
		_container.UDPConnectionLimit = 2;
	}

	public Task Start()
	{
		_container.Start();
		return Task.CompletedTask;
	}

	public Task Stop()
	{
		_container.Stop();
		return Task.CompletedTask;
	}

	private void ConnectionEstablished(Connection connection, ConnectionType connectionType)
	{
		Log.Debug($"New connection from {connection.IPRemoteEndPoint} | {connectionType}");

		//TODO REGISTER PACKET
		//connection.RegisterStaticPacketHandler<REQUESTCLASS>(OnReceive);
	}

	private void OnReceive<T>(T packet, Connection connection)
	{
		_handlerInvoker.Invoke(packet?.GetType(), packet, connection);
	}

	private void ConnectionLost(Connection connection, ConnectionType connectionType, CloseReason closeReason)
	{
		Log.Debug($"Connection closed from {connection.IPRemoteEndPoint}	|	{connectionType}	|	Reason : {closeReason}");
	}
}