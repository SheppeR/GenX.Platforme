using Microsoft.EntityFrameworkCore;

namespace GenX.Server.Database;

public class ServerContext : DbContext
{
	public ServerContext(DbContextOptions options) : base(options)
	{
	}

	public bool IsAlive()
	{
		try
		{
			Database.OpenConnection();
			Database.CloseConnection();
		}
		catch (Exception)
		{
			return false;
		}

		return true;
	}
}