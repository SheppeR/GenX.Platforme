using Network.Attributes;
using Network.Packets;

namespace GenX.Network.Packets.FriendsDatas;

[PacketRequest(typeof(DenyFriendRequest))]
public class DenyFriendResponse(RequestPacket request, int id) : ResponsePacket(request)
{
    public int FriendID { get; set; } = id;
}