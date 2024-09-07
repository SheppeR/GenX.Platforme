namespace GenX.Network.Server;

public interface IGenXServer
{
	Task Start();

	Task Stop();
}