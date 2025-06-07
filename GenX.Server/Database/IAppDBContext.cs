using Microsoft.EntityFrameworkCore;

namespace GenX.Server.Database;

public interface IAppDBContext
{
    public DbSet<DbUser> DbUser { get; set; }

    Task Migrate();

    bool Exists();

    bool IsAlive();
}