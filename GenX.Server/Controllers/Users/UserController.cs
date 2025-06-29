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
                loginResult = LoginResult.LoginFail;
            else
                loginResult = user.IsBanned ? LoginResult.AccountBanned : LoginResult.LoginSuccess;
        }

        if (loginResult == LoginResult.LoginSuccess && user != null)
        {
            user.LastLoginTime = DateTime.UtcNow;
            user.Status = 1;
            appDbContext.SaveChanges();
        }

        return Task.FromResult((loginResult, user));
    }

    public async Task<bool> LogOutUserAsync(DbUser? user)
    {
        if (user != null)
        {
            user.LastLogoutTime = DateTime.UtcNow;
            user.OnlineTime += (DateTime.UtcNow - user.LastLoginTime).TotalSeconds;
            user.Status = 0;
            await appDbContext.SaveChanges();
        }

        return true;
    }
}