using Network.Packets;

namespace GenX.Network.Packets.Login;

public class LoginRequest(string login, string? passwordHash) : RequestPacket
{
    public string? PasswordHash { get; set; } = passwordHash;

    public string Login { get; set; } = login;
}