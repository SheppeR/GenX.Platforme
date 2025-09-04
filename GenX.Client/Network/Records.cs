using GenX.Network.Packets.FriendsDatas;
using GenX.Network.Packets.UserDatas;

namespace GenX.Client.Network;

public record FriendsDatasMessage(FriendsDatasResponse response);

public record UserDatasMessage(UserDatasResponse response);