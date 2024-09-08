using GenX.Network.Packets.Login;
using GenX.Server.Database;
using Microsoft.EntityFrameworkCore;
using Network;
using Sylver.HandlerInvoker.Attributes;

namespace GenX.Server.Handlers;

[Handler]
public class LoginHandler
{
    private readonly IServerContext _context;

    public LoginHandler(IServerContext context)
    {
	    _context = context;
    }

    [HandlerAction(typeof(LoginRequest))]
    public async void OnHandle(LoginRequest request, Connection client)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u=> u.Username != null && u.Username.Equals(request.Username));

        LoginResult loginResult;

        if (user == null)
        {
            loginResult = LoginResult.AccountNotFound;
        }
        else
        {
            if (user.Password != null && !user.Password.ToUpper().Equals(request.PasswordHash.ToUpper()))
            {
                loginResult = LoginResult.LoginFail;
            }
            else
            {
                loginResult = user.Banned ? LoginResult.AccountBanned : LoginResult.LoginSuccess;
            }
        }

        client.Send(new LoginResponse(loginResult, request));
    }
}