using Network.Attributes;
using Network.Packets;

namespace GenX.Network.Packets.Login;

[PacketRequest(typeof(LoginRequest))]
public class LoginResponse(LoginResult result, RequestPacket request) : ResponsePacket(request)
{
    public LoginResult Result { get; set; } = result;
}