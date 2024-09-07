using Network.Attributes;
using Network.Packets;

namespace GenX.Network.Packets.Login;

[PacketRequest(typeof(LoginRequest))]
public class LoginResponse : ResponsePacket
{
	public LoginResponse(LoginResult result, RequestPacket request)
		: base(request)
	{
		Result = result;
	}

	public LoginResult Result { get; set; }
}