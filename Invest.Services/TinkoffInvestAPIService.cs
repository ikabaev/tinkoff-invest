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

            //investApi.Users.GetAccounts().Accounts[0].

            var history = investApi.HistoryData();
            var figi = history.Gigi().Result;

            int j = 0;
            foreach ( var id in figi) 
            {
                j++;
                logger.LogInformation("{0}/{1} loading figi: {2}...", j, figi.Length, id);

                for (var i = 0; i < 10; i++)
                {
                    var year = DateTime.Now.Year - i;
                    
                    var data = history.HistoryData(id, year).Result;
                    if (data?.Length > 0)
                    {
                        var dirPath = @$"C:\tinkoff-history\{id}\{year}";
                        if (!Directory.Exists(dirPath))
                            Directory.CreateDirectory(dirPath);

                        var dataPath = Path.Combine(dirPath, $"{id}.zip");

                        File.WriteAllBytes(dataPath, data);
                    }
                }
            }
            
            return Task.CompletedTask;
        }
    }
}
