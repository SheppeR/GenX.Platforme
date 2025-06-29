using GenX.Network.Packets.Logout;
using GenX.Server.Controllers.Users;
using GenX.Server.Database;
using Network;
using Sylver.HandlerInvoker.Attributes;

namespace GenX.Server.Network.Handlers;

[Handler]
public class LogoutHandler(IUserController userController)
{
    [HandlerAction(typeof(LogoutRequest))]
    public async void OnHandle(LogoutRequest request, Connection client, DbUser? user)
    {
        var logoutResult = user == null || await userController.LogOutUserAsync(user);

        client.Send(new LogoutResponse(logoutResult, request));
    }
}