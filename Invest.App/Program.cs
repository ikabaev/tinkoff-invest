using Invest.App;
using Invest.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

var host = builder
    .ConfigureHostConfiguration(context => context.AddUserSecrets<Program>())
    .ConfigureServices((context, services) =>
    {
        var settings = new AppSettings();
        context.Configuration.Bind(settings);
        foreach(var setting in settings.Providers)
        {
            var provider = ProviderFactory.Create(setting);

            services
                .AddHostedService<InvestBackgroundService>();
                //.AddInvestApiClient(setting.AppName ?? $"{Guid.NewGuid()}", (_, settings) =>
                //{
                //    settings.Sandbox = false;
                //    settings.AppName = setting.AppName;
                //    settings.AccessToken = setting.AccessToken;
                //});

            //TinkoffInvestAPIServiceExtention.accessToken = setting.AccessToken ?? string.Empty;
        }
    })
    .Build();
    
await host.RunAsync();

// TODO: +добавить в секрет боевой токен
// TODO: +хранить в секретах нескольео токенов
// TODO: +используя список figi последовательно загрузить