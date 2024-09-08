using System.Diagnostics;
using GenX.Server.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace GenX.Server.Database;

public class ServerContext : DbContext, IServerContext
{
	private readonly IConfiguration _configuration;

	public ServerContext(DbContextOptions options, IConfiguration configuration) : base(options)
	{
		_configuration = configuration;
	}

	public DbSet<DbUser> Users { get; set; }

	public async void Migrate()
	{
		await Database.MigrateAsync();
	}

	public async Task<bool> Exists()
	{
		return await (this.GetService<IDatabaseCreator>() as RelationalDatabaseCreator)!.ExistsAsync();
	}

	public async Task<bool> IsAlive()
	{
		try
		{
			await Database.OpenConnectionAsync();
			await Database.CloseConnectionAsync();
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