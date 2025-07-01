using GenX.Network.Packets.Login;
using GenX.Server.Controllers.Users;
using Network;
using Sylver.HandlerInvoker.Attributes;

namespace GenX.Server.Network.Handlers;

[Handler]
public class LoginHandler(IUserController userController, IGenXServer server)
{
    [HandlerAction(typeof(LoginRequest))]
    public async void OnHandle(LoginRequest request, Connection client)
    {
        var datas = await userController.LogInUserAsync(request.Login, request.PasswordHash);

        if (datas.user != null)
        {
            server[client] = datas.user;
        }

        //TODO BROADCAST TO ALL FRIENDS USER STATUS
        client.Send(new LoginResponse(datas.loginResult, request));
    }
}