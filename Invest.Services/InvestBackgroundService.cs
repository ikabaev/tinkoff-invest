using Invest.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Invest.Services
{
    public class InvestBackgroundService(ILogger<InvestBackgroundService> logger, IInvestProvider investApi, IHostApplicationLifetime lifetime) : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            return Task.CompletedTask;
        }
    }
}
