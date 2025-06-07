using Network.Attributes;
using Network.Packets;

namespace GenX.Network.Packets.Logout;

[PacketRequest(typeof(LogoutRequest))]
public class LogoutResponse(RequestPacket request) : ResponsePacket(request);