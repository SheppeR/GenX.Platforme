using GenX.Network.Packets.FriendsDatas;
using GenX.Server.Controllers.Users;
using Network;
using Sylver.HandlerInvoker.Attributes;

namespace GenX.Server.Network.Handlers;

[Handler]
public class FriendsDatasHanler(IUserController userController)
{
    [HandlerAction(typeof(FriendsDatasRequest))]
    public void OnHandle(FriendsDatasRequest request, Connection client)
    {
        userController.SendFriendsDatas(request, client);
    }
}