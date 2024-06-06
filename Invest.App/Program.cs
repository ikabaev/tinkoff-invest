using Invest.App;
using Invest.Services;
using Invest.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

var host = builder
    .ConfigureHostConfiguration(context => context.AddUserSecrets<Program>())
    .ConfigureServices((context, services) =>
    {
        // var settings = new AppSettings();
        var settings = context.Configuration.Get<AppSettings>() ?? new();
        foreach(var setting in settings.Providers)
        {
            var investProvider = ProviderFactory.Create(setting) as IInvestProvider;

            if (investProvider is null)
                throw new ArgumentNullException(nameof(investProvider));

            services
                .AddSingleton(investProvider)
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