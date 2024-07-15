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
            var investProvider = ProviderFactory.Create(setting);

            if (investProvider is null)
                throw new NullReferenceException(nameof(investProvider));

            var itypes = investProvider.GetType().GetInterfaces();

            if (itypes.Length == 0)
                throw new NullReferenceException($"Тип {investProvider.GetType()} не реализует ни одного интерфейса");

            foreach (var i in itypes)
                services
                    .AddSingleton(i, investProvider);
        }

        services
                .AddHostedService<InvestBackgroundService>();
    })
    .Build();


await host.RunAsync();