using GenX.Network.Packets.FriendsDatas;
using GenX.Network.Packets.UserDatas;
using iNKORE.UI.WPF.Modern.Controls;

namespace GenX.Client.Network;

public record FriendsDatasMessage(FriendsDatasResponse response);

public record UserDatasMessage(UserDatasResponse response);

public record AlertMessage(string message, string title, InfoBarSeverity severity);