using GenX.Client.Options;
using GenX.Network.Packets.Login;
using GenX.Network.Packets.Logout;
using Microsoft.Extensions.Options;
using Network;
using Network.Enums;
using Network.Packets;
using Serilog;
using Sylver.HandlerInvoker;

namespace GenX.Client.Network;

public class GenXClient : IGenXClient
{
    private readonly ClientConnectionContainer _container;
    private readonly IHandlerInvoker _handlerInvoker;

    public GenXClient(IHandlerInvoker handlerInvoker, IOptions<ServerInfos> serverInfos)
    {
        _handlerInvoker = handlerInvoker;

        _container = ConnectionFactory.CreateClientConnectionContainer(serverInfos.Value.Host, serverInfos.Value.Port);
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
        connection.RegisterStaticPacketHandler<LoginResponse>(OnReceive);
        connection.RegisterStaticPacketHandler<LogoutResponse>(OnReceive);
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

    private void OnConnectionLost(Connection connection, ConnectionType connectionType, CloseReason closeReason)
    {
        Log.Debug($"Connection closed from {connection.IPRemoteEndPoint} | Reason : {closeReason}");
    }
}