using CommunityToolkit.Mvvm.Messaging;
using GenX.Network.Packets.FriendsDatas;
using Network;
using Sylver.HandlerInvoker.Attributes;

namespace GenX.Client.Network.Handlers;

[Handler]
public class FriendsDatasHanler(IMessenger messenger)
{
    [HandlerAction(typeof(FriendsDatasResponse))]
    public void OnHandle(FriendsDatasResponse response, Connection connection)
    {
        messenger.Send(new FriendsDatasMessage(response));
    }
}