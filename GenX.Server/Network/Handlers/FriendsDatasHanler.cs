using GenX.Network.Packets.FriendsDatas;
using GenX.Server.Controllers.Friends;
using Network;
using Sylver.HandlerInvoker.Attributes;

namespace GenX.Server.Network.Handlers;

[Handler]
public class FriendsDatasHanler(IFriendController friendController, IGenXServer server)
{
    [HandlerAction(typeof(FriendsDatasRequest))]
    public async void OnHandle(FriendsDatasRequest request, Connection client)
    {
        var friends = await friendController.GetFriendsAsync(server[client].ID);
        var friendsPending = await friendController.GetPendingFriendRequestsAsync(server[client].ID);

        var rep = new FriendsDatasResponse(request)
        {
            FriendsData = friends.Select(f => new FriendDatas
            {
                ID = f.ID,
                Pseudo = f.Pseudo,
                Status = f.Status
            }).ToList(),
            FriendsPendingData = friendsPending.Select(f => new FriendDatas
            {
                ID = f.ID,
                Pseudo = f.Pseudo,
                Status = f.Status
            }).ToList()
        };

        client.Send(rep);
    }
}