using GenX.Network.Packets.FriendsDatas;
using GenX.Network.Packets.Login;
using GenX.Network.Packets.Logout;
using GenX.Network.Packets.UserDatas;
using GenX.Server.Database;
using Network;

namespace GenX.Server.Controllers.Users;

public interface IUserController
{
    void TryToLogInUser(LoginRequest request, Connection client);

    void TryToLogOutUser(LogoutRequest request, DbUser user);

    void SendUserDatas(UserDatasRequest request, Connection client);

    void SendFriendsDatas(FriendsDatasRequest request, Connection client);
}