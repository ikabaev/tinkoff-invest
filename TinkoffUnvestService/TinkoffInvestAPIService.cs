using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tinkoff.InvestApi;

namespace InvestAPI.Services
{
    public class TinkoffInvestAPIService: BackgroundService
    {
        private readonly InvestApiClient _investApi;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly ILogger<TinkoffInvestAPIService> _logger;
        public TinkoffInvestAPIService(ILogger<TinkoffInvestAPIService> logger, InvestApiClient investApi, IHostApplicationLifetime lifetime)
        {
            _logger = logger;
            _investApi = investApi;
            _lifetime = lifetime;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var accounts = _investApi.Sandbox.GetSandboxAccounts(
                 new Tinkoff.InvestApi.V1.GetAccountsRequest()
             );

            if (accounts.Accounts.Count == 0)
            {
                var acc = _investApi.Sandbox.OpenSandboxAccount(
                    new Tinkoff.InvestApi.V1.OpenSandboxAccountRequest()
                );
                var accId = acc.AccountId;
            }

            return Task.CompletedTask;
        }
    }
}
