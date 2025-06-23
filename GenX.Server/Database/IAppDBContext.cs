using Microsoft.EntityFrameworkCore;

namespace GenX.Server.Database;

public interface IAppDBContext
{
    public DbSet<DbUser> DbUser { get; set; }

    public DbSet<DbFriend> DbFriend { get; set; }

    Task Migrate();

    bool Exists();

    bool IsAlive();

    Task<int> SaveChanges();
}