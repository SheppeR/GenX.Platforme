using System.Globalization;
using GenX.Network.Packets.UserDatas;
using Network;
using Sylver.HandlerInvoker.Attributes;

namespace GenX.Server.Network.Handlers;

[Handler]
public class UserDatasHandler(IGenXServer server)
{
    [HandlerAction(typeof(UserDatasRequest))]
    public void OnHandle(UserDatasRequest request, Connection client)
    {
        var rep = new UserDatasResponse(request)
        {
            UserID = server[client].ID,
            Access = server[client].Access,
            CreationDate = server[client].CreationDate.ToString(CultureInfo.CurrentCulture),
            LastLoginTime = server[client].LastLoginTime.ToString(CultureInfo.CurrentCulture),
            LastLogoutTime = server[client].LastLogoutTime.ToString(CultureInfo.CurrentCulture),
            OnlineTime = server[client].OnlineTime,
            Pseudo = server[client].Pseudo,
            Status = server[client].Status
        };

        client.Send(rep);
    }
}