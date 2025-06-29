using GenX.Network.Packets.FriendsDatas;
using GenX.Server.Controllers.Friends;
using GenX.Server.Database;
using Network;
using Sylver.HandlerInvoker.Attributes;

namespace GenX.Server.Network.Handlers;

[Handler]
public class FriendsDatasHanler(IFriendController friendController)
{
    [HandlerAction(typeof(FriendsDatasRequest))]
    public async void OnHandle(FriendsDatasRequest request, Connection client, DbUser user)
    {
        var friends = await friendController.GetFriendsAsync(user.ID);

        var rep = new FriendsDatasResponse(request)
        {
            FriendsData = friends.Select(f => new FriendDatas
            {
                ID = f.Friend.ID,
                Pseudo = f.Friend.Pseudo,
                Status = f.Friend.Status, Avatar = "https://genx.example.com/avatars/", IsAccepted = f.IsAccepted
            }).ToList()
        };

        client.Send(rep);
    }
}