using Network.Packets;

namespace GenX.Network.Packets.Login;

public class LoginRequest : RequestPacket
{
	public LoginRequest(string login, string passwordHash)
	{
		Login = login;
		PasswordHash = passwordHash;
	}

	public string PasswordHash { get; set; }

	public string Login { get; set; }
}