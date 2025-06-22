using GenX.Network.Packets.UserDatas;
using GenX.Server.Controllers.Users;
using Network;
using Sylver.HandlerInvoker.Attributes;

namespace GenX.Server.Network.Handlers;

[Handler]
public class UserDatasHandler(IUserController userController)
{
    [HandlerAction(typeof(UserDatasRequest))]
    public void OnHandle(UserDatasRequest request, Connection client)
    {
        userController.SendUserDatas(request, client);
    }
}