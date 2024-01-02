using Invest.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

var host = builder
    .ConfigureHostConfiguration(context => context.AddUserSecrets<Program>())
    .ConfigureServices((context, services) =>
    {
        var tinkoffApi = new InvestProviders();
        context.Configuration.Bind(tinkoffApi);
        foreach(var account in tinkoffApi.Tinkoff ?? [])
        {
            services
                .AddHostedService<TinkoffInvestAPIService>()
                .AddInvestApiClient(account.AppName ?? $"{Guid.NewGuid()}", (_, settings) => 
                {
                    settings.Sandbox = account.Sandbox;
                    settings.AppName = account.AppName;
                    settings.AccessToken = account.AccessToken;
                });

            TinkoffInvestAPIServiceExtention.accessToke = account.AccessToken ?? string.Empty;
        }
    })
    .Build();

await host.RunAsync();

// TODO: добавить в секрет боевой токен
// TODO: хранить в секретах нескольео токенов
// TODO: используя список figi последовательно загрузить проанализировать отдельнжо и совместно