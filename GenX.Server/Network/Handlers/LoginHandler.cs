using GenX.Network.Packets.Login;
using GenX.Server.Controllers;
using Network;
using Sylver.HandlerInvoker.Attributes;

namespace GenX.Server.Network.Handlers;

[Handler]
public class LoginHandler(IUserController userController)
{
    [HandlerAction(typeof(LoginRequest))]
    public void OnHandle(LoginRequest request, Connection client)
    {
        userController.TryToLogInUser(request, client);
    }
}