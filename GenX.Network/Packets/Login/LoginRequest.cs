using Network.Packets;

namespace GenX.Network.Packets.Login;

public class LoginRequest(string username, string? passwordHash) : RequestPacket
{
    public string? PasswordHash { get; set; } = passwordHash;

    public string Username { get; set; } = username;
}