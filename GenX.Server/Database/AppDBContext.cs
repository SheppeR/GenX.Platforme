using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Serilog;

namespace GenX.Server.Database;

public class AppDBContext(DbContextOptions<AppDBContext> options, IConfiguration configuration)
    : DbContext(options), IAppDBContext
{
    public DbSet<DbUser> DbUser { get; set; }

    public DbSet<DbFriend> DbFriend { get; set; }

    public async Task Migrate()
    {
        Log.Debug("Checking migration for the database ...");
        await Database.MigrateAsync();
    }

    public bool Exists()
    {
        return ((RelationalDatabaseCreator)this.GetService<IDatabaseCreator>()).Exists();
    }

    public bool IsAlive()
    {
        try
        {
            Database.OpenConnection();
            Database.CloseConnection();
        }
        catch (MySqlException)
        {
            return false;
        }

        return true;
    }

    public new async Task<int> SaveChanges()
    {
        return await SaveChangesAsync();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            try
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
            catch (Exception e)
            {
                Log.Error($"Cannot start server because of mysql exception: {Environment.NewLine}{e.InnerException}");
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DbUser>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.Login).IsRequired();
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.Pseudo).IsRequired();
        });

        modelBuilder.Entity<DbFriend>()
            .HasOne(f => f.User)
            .WithMany(u => u.Friends)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DbFriend>()
            .HasOne(f => f.Friend)
            .WithMany(u => u.FriendOf)
            .HasForeignKey(f => f.FriendId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}