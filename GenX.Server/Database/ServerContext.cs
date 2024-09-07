using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GenX.Server.Database;

public class ServerContext : DbContext
{
	private readonly IConfiguration _configuration;

	public ServerContext(DbContextOptions options, IConfiguration configuration) : base(options)
	{
		_configuration = configuration;
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

	protected override void OnConfiguring(DbContextOptionsBuilder options)
	{
		string? connectionString;
		Version version;

		if (Debugger.IsAttached)
		{
			connectionString = _configuration.GetConnectionString("db_core_local");
			version = new Version(11, 2, 2);
		}
		else
		{
			connectionString = _configuration.GetConnectionString("db_core");
			version = new Version(10, 11, 9);
		}

		options.UseMySql(connectionString, new MariaDbServerVersion(version));
	}
}