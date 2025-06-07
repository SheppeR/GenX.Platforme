using GenX.Network.Packets.Login;
using GenX.Network.Packets.Logout;
using GenX.Server.Database;
using GenX.Server.Network;
using Network;

namespace GenX.Server.Controllers;

public class UserController(IAppDBContext appDbContext, IGenXServer server) : IUserController
{
    public void TryToLogInUser(LoginRequest request, Connection client)
    {
        var user = appDbContext.DbUser.FirstOrDefault(u => u.Login!.Equals(request.Username));

        LoginResult loginResult;

        if (user == null)
        {
            loginResult = LoginResult.AccountNotFound;
        }
        else
        {
            if (!user.PasswordHash!.ToUpperInvariant().Equals(request.PasswordHash?.ToUpperInvariant()))
            {
                loginResult = LoginResult.LoginFail;
            }
            else
            {
                if (user.IsBanned)
                {
                    loginResult = LoginResult.AccountBanned;
                }
                else
                {
                    loginResult = LoginResult.LoginSuccess;
                    server[client] = user;
                }
            }
        }

        client.Send(new LoginResponse(loginResult, request));

        //TODO BROADCAST TO ALL FRIENDS USER STATUS
    }

    public void TryToLogOutUser(LogoutRequest request, DbUser user)
    {
    }
}