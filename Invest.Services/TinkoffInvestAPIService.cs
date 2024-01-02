using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tinkoff.InvestApi;

namespace Invest.Services
{
    public class TinkoffInvestAPIService(ILogger<TinkoffInvestAPIService> logger, InvestApiClient investApi, IHostApplicationLifetime lifetime) : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var accounts = investApi.Sandbox.GetSandboxAccounts(
                 new Tinkoff.InvestApi.V1.GetAccountsRequest()
             );

            if (accounts.Accounts.Count == 0)
            {
                var acc = investApi.Sandbox.OpenSandboxAccount(
                    new Tinkoff.InvestApi.V1.OpenSandboxAccountRequest()
                );
                var accId = acc.AccountId;
            }

            var history = investApi.HistoryData();
            var figi = history.Gigi().Result;

            foreach ( var id in figi) 
            {
                var data = history.HistoryData(id, 2023).Result;
                if (data?.Length > 0) {
                    File.WriteAllBytes(@$"C:\tinkoff-history\{id}.zip", data);
                }
            }
            
            return Task.CompletedTask;
        }
    }
}
