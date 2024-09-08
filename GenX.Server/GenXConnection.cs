using GenX.Server.Database.Entities;
using Network;

namespace GenX.Server;

public abstract class GenXConnection : Connection
{
	public DbUser? User { get; set; }
}