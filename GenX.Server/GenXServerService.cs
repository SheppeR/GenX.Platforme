using GenX.Common.Helpers.Logger;
using GenX.Server.Database;
using GenX.Server.Network;
using Microsoft.Extensions.Hosting;

namespace GenX.Server;

public class GenXServerService(IGenXServer genXServer, IAppDBContext appDbContext) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        SerilogUtils.PrintSection("DATABASE");
        await appDbContext.Migrate();

        SerilogUtils.PrintSection("NETWORK");
        await genXServer.Start();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await genXServer.Stop();
    }
}