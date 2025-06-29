using System.Globalization;
using GenX.Network.Packets.UserDatas;
using GenX.Server.Database;
using Network;
using Sylver.HandlerInvoker.Attributes;

namespace GenX.Server.Network.Handlers;

[Handler]
public class UserDatasHandler
{
    [HandlerAction(typeof(UserDatasRequest))]
    public void OnHandle(UserDatasRequest request, Connection client, DbUser user)
    {
        var rep = new UserDatasResponse(request)
        {
            UserID = user.ID,
            Access = user.Access,
            CreationDate = user.CreationDate.ToString(CultureInfo.CurrentCulture),
            LastLoginTime = user.LastLoginTime.ToString(CultureInfo.CurrentCulture),
            LastLogoutTime = user.LastLogoutTime.ToString(CultureInfo.CurrentCulture),
            OnlineTime = user.OnlineTime,
            Pseudo = user.Pseudo,
            Status = user.Status
        };

        client.Send(rep);
    }
}