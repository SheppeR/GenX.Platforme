using GenX.Network.Packets.Login;
using GenX.Server.Database;
using Microsoft.EntityFrameworkCore;
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
	public async void OnHandle(LoginRequest request, GenXConnection client)
	{
		var user = await _context.Users.FirstOrDefaultAsync(u => u.Username != null && u.Username.Equals(request.Username));

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
				if (user.Banned)
				{
					loginResult = LoginResult.AccountBanned;
				}
				else
				{
					loginResult = LoginResult.LoginSuccess;
					client.User = user;
				}
			}
		}

		client.Send(new LoginResponse(loginResult, request));
	}
}
