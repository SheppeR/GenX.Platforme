using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenX.Server.Database;

public class DbUser
{
    public int ID { get; set; }

    [Column(TypeName = "VARCHAR")]
    [MaxLength(128)]
    public string? Login { get; set; }

    [Column(TypeName = "VARCHAR")]
    [MaxLength(128)]
    public string? PasswordHash { get; set; }

    [Column(TypeName = "VARCHAR")]
    [MaxLength(128)]
    public string? Email { get; set; }

    [Column(TypeName = "VARCHAR")]
    [MaxLength(128)]
    public string? Pseudo { get; set; }

    public bool IsBanned { get; set; }

    public int Status { get; set; }

    public int Access { get; set; }

    public double OnlineTime { get; set; }

    public DateTime LastLogoutTime { get; set; }

    public DateTime LastLoginTime { get; set; }

    public DateTime CreationDate { get; set; }

    public List<DbFriend> Friends { get; set; } = [];

    public List<DbFriend> FriendOf { get; set; } = [];

    public string? Avatar { get; set; }
}