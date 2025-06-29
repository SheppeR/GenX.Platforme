using GenX.Network.Packets.FriendsDatas;
using GenX.Server.Controllers.Friends;
using GenX.Server.Database;
using Network;
using Sylver.HandlerInvoker.Attributes;

namespace GenX.Server.Network.Handlers;

[Handler]
public class AcceptFriendHandler(IFriendController friendController)
{
    [HandlerAction(typeof(AcceptFriendRequest))]
    public async void OnHandle(AcceptFriendRequest request, Connection client, DbUser user)
    {
        await friendController.AcceptFriendAsync(user.ID, request.FriendID);
        client.Send(new AcceptFriendResponse(request, request.FriendID));
    }
}