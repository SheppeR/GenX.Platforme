using GenX.Common.Options;
using GenX.Network.Packets.Login;
using GenX.Network.Packets.Logout;
using GenX.Server.Database;
using GenX.Server.Options;
using Network;
using Network.Enums;
using Serilog;
using Sylver.HandlerInvoker;

namespace GenX.Server.Network;

public class GenXServer : IGenXServer
{
    private readonly Dictionary<Connection, DbUser> _connections;
    private readonly ServerConnectionContainer _container;
    private readonly IHandlerInvoker _handlerInvoker;
    private readonly IServiceProvider _serviceProvider;

    public GenXServer(IHandlerInvoker handlerInvoker, IServiceProvider serviceProvider,
        IWritableOptions<ServerInfos> configuration)
    {
        _handlerInvoker = handlerInvoker;
        _serviceProvider = serviceProvider;
        _connections = new Dictionary<Connection, DbUser>();

        _container = ConnectionFactory.CreateServerConnectionContainer(configuration.Value.Port, false);
        _container.ConnectionLost += ConnectionLost;
        _container.ConnectionEstablished += ConnectionEstablished;
        _container.AllowUDPConnections = true;
        _container.UDPConnectionLimit = 2;
    }

    public Task Start()
    {
        Log.Information($"Starting Server on {_container.Port}");

        _container.Start();

        return Task.CompletedTask;
    }

    public Task Stop()
    {
        Log.Information("Stoping Server");

        _container.Stop();
        return Task.CompletedTask;
    }

    public DbUser this[Connection key]
    {
        get => _connections[key];
        set => _connections[key] = value;
    }

    private void ConnectionEstablished(Connection connection, ConnectionType connectionType)
    {
        Log.Debug($"New connection from {connection.IPRemoteEndPoint} | {connectionType}");

        _connections.Add(connection, null!);

        //TODO REGISTER PACKET
        connection.RegisterStaticPacketHandler<LoginRequest>(OnReceive);
        connection.RegisterStaticPacketHandler<LogoutRequest>(OnReceive);
    }

    private void OnReceive<T>(T packet, Connection connection)
    {
        try
        {
            if (packet?.GetType() == typeof(LoginRequest))
                _handlerInvoker.Invoke(packet.GetType(), packet, connection);
            else
                _handlerInvoker.Invoke(packet?.GetType(), packet, this[connection]);
        }
        catch (Exception)
        {
            Log.Error($"Unknow Packet {packet?.GetType()} received from {connection.IPRemoteEndPoint.Address}");
        }
    }

    private void ConnectionLost(Connection connection, ConnectionType connectionType, CloseReason closeReason)
    {
        Log.Debug($"Connection closed from {connection.IPRemoteEndPoint}	|	{connectionType}	|	Reason : {closeReason}");
    }
}