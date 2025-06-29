using GenX.Network.Packets.FriendsDatas;
using GenX.Server.Controllers.Friends;
using GenX.Server.Database;
using Network;
using Sylver.HandlerInvoker.Attributes;

namespace GenX.Server.Network.Handlers;

[Handler]
public class DenyFriendHandler(IFriendController friendController)
{
    [HandlerAction(typeof(DenyFriendRequest))]
    public async void OnHandle(DenyFriendRequest request, Connection client, DbUser user)
    {
        await friendController.DenyFriendAsync(user.ID, request.FriendID);
        client.Send(new DenyFriendResponse(request, request.FriendID));
    }
}