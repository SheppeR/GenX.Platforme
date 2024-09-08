using Network.Packets;

namespace GenX.Network.Packets.Login;

public class LoginRequest : RequestPacket
{
	public LoginRequest(string username, string passwordHash)
	{
		Username = username;
		PasswordHash = passwordHash;
	}

	public string PasswordHash { get; set; }

	public string Username { get; set; }
}