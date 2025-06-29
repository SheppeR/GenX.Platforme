using Network.Attributes;
using Network.Packets;

namespace GenX.Network.Packets.Logout;

[PacketRequest(typeof(LogoutRequest))]
public class LogoutResponse(bool logoutResult, RequestPacket request) : ResponsePacket(request)
{
    public bool Success { get; set; } = logoutResult;
}