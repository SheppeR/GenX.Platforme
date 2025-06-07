using GenX.Server.Database;
using Network;

namespace GenX.Server.Network;

public interface IGenXServer
{
    DbUser this[Connection key] { get; set; }

    Task Start();

    Task Stop();
}