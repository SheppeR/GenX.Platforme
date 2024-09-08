using GenX.Server.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace GenX.Server.Database;

public interface IServerContext : IDisposable
{
	DbSet<DbUser> Users { get; }

	int SaveChanges();

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

	void Migrate();

	Task<bool> Exists();

	Task<bool> IsAlive();
}