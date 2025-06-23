using GenX.Network.Packets.Login;
using GenX.Server.Database;

namespace GenX.Server.Controllers.Users;

public interface IUserController
{
    Task<(LoginResult loginResult, DbUser? user)> LogInUserAsync(string login, string? password);

    Task LogOutUserAsync(DbUser user);
}