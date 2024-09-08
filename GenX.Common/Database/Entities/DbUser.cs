using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenX.Common.Database.Entities;

[Table("users")]
public record DbUser : DbEntity
{
	public DbUser()
	{
		CreatedAt = DateTime.UtcNow;
	}

	[Required] public string? Username { get; init; }

	[Required] public string? Password { get; init; }

	[Required] public string? Email { get; init; }

	[Required] public string? Pseudo { get; init; }

	[Required] public string? AvatarHash { get; init; }

	[Required]
	[Column(TypeName = "BIT")]
	[DefaultValue(false)]
	public bool EmailConfirmed { get; init; }

	[Required]
	[Column(TypeName = "BIT")]
	[DefaultValue(false)]
	public bool Banned { get; init; }

	[Required]
	[Column(TypeName = "DATETIME")]
	public DateTime CreatedAt { get; private init; }

	[Required]
	[Column(TypeName = "DATETIME")]
	public DateTime LastLoginTime { get; init; }

	[Required]
	[Column(TypeName = "DATETIME")]
	public DateTime LastLogoutTime { get; init; }

	[Required] [DefaultValue(0)] public int Authority { get; init; }

	[Required] [DefaultValue(0)] public int Status { get; init; }

	[Required] [DefaultValue(0D)] public long OnlineTime { get; init; }
}