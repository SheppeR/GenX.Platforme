namespace GenX.Server.Database;

public class DbFriend
{
    public int ID { get; set; }

    public int UserId { get; set; }
    public DbUser User { get; set; } = null!;

    public int FriendId { get; set; }
    public DbUser Friend { get; set; } = null!;

    public bool IsAccepted { get; set; }

    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
}