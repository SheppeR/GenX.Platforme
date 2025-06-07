using GenX.Network.Packets.Login;
using GenX.Network.Packets.Logout;
using GenX.Server.Database;
using Network;

namespace GenX.Server.Controllers;

public interface IUserController
{
    void TryToLogInUser(LoginRequest request, Connection client);

    void TryToLogOutUser(LogoutRequest request, DbUser user);
}