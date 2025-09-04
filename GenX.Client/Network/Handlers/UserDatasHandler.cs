using CommunityToolkit.Mvvm.Messaging;
using GenX.Network.Packets.UserDatas;
using Network;
using Sylver.HandlerInvoker.Attributes;

namespace GenX.Client.Network.Handlers;

[Handler]
public class UserDatasHandler(IMessenger messenger)
{
    [HandlerAction(typeof(UserDatasResponse))]
    public void OnHandle(UserDatasResponse response, Connection connection)
    {
        messenger.Send(new UserDatasMessage(response));
    }
}