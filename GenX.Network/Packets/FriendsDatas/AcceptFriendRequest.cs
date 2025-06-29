using Network.Packets;

namespace GenX.Network.Packets.FriendsDatas;

public class AcceptFriendRequest(int id) : RequestPacket
{
    public int FriendID { get; set; } = id;
}