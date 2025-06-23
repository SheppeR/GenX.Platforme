using GenX.Network.Packets.Login;
using GenX.Server.Database;

namespace GenX.Server.Controllers.Users;

public class UserController(IAppDBContext appDbContext) : IUserController
{
    public Task<(LoginResult loginResult, DbUser? user)> LogInUserAsync(string login, string? password)
    {
        var user = appDbContext.DbUser.FirstOrDefault(u => u.Login!.Equals(login));

        LoginResult loginResult;

        if (user == null)
        {
            loginResult = LoginResult.AccountNotFound;
        }
        else
        {
            if (!user.PasswordHash!.ToUpperInvariant().Equals(password?.ToUpperInvariant()))
            {
                loginResult = LoginResult.LoginFail;
            }
            else
            {
                loginResult = user.IsBanned ? LoginResult.AccountBanned : LoginResult.LoginSuccess;
            }
        }

        //TODO BROADCAST TO ALL FRIENDS USER STATUS
        return Task.FromResult((loginResult, user));
    }

    public Task LogOutUserAsync(DbUser user)
    {
        return null!;
    }
}