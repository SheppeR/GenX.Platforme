using Network.Attributes;
using Network.Packets;

namespace GenX.Network.Packets.FriendsDatas;

[PacketRequest(typeof(FriendsDatasRequest))]
public class FriendsDatasResponse(RequestPacket request) : ResponsePacket(request)
{
    public List<FriendDatas>? FriendsData { get; set; }
}