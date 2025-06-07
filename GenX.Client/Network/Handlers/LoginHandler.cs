using GenX.Network.Packets.Login;
using Network;
using Sylver.HandlerInvoker.Attributes;

namespace GenX.Client.Network.Handlers;

[Handler]
public class LoginHandler
{
    [HandlerAction(typeof(LoginResponse))]
    public void OnHandle(LoginRequest request, Connection client)
    {
    }
}