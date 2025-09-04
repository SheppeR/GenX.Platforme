using Network.Attributes;
using Network.Packets;

namespace GenX.Network.Packets.UserDatas;

[PacketRequest(typeof(UserDatasRequest))]
public class UserDatasResponse(RequestPacket request) : ResponsePacket(request)
{
    public int UserID { get; set; }
    public int Access { get; set; }
    public string? CreationDate { get; set; }
    public string? LastLoginTime { get; set; }
    public string? LastLogoutTime { get; set; }
    public double OnlineTime { get; set; }
    public string? Pseudo { get; set; }
    public int Status { get; set; }
    public string? Avatar { get; set; }
    public string? Event { get; set; }
}