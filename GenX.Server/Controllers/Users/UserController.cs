using System.Globalization;
using GenX.Network.Packets.FriendsDatas;
using GenX.Network.Packets.Login;
using GenX.Network.Packets.Logout;
using GenX.Network.Packets.UserDatas;
using GenX.Server.Database;
using GenX.Server.Network;
using Network;

namespace GenX.Server.Controllers.Users;

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

    public void SendUserDatas(UserDatasRequest request, Connection client)
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

    public void SendFriendsDatas(FriendsDatasRequest request, Connection client)
    {
        var rep = new FriendsDatasResponse(request);

        client.Send(rep);
    }
}