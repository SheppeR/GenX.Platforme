using Network.Enums;
using Network.Packets;

namespace GenX.Client.Network;

public interface IGenXClient
{
    Task<bool> IsConnected { get; }

    Task Disconnect(CloseReason reason);

    void Send(RequestPacket request);

    Task<T> SendAndReceive<T>(RequestPacket request) where T : ResponsePacket;
}