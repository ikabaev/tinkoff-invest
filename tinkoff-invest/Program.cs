using InvestAPI.Services;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

var host = builder
    .ConfigureHostConfiguration(context => 
        context.AddUserSecrets<Program>())
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<TinkoffInvestAPIService>();
        services.AddInvestApiClient((_, settings) => context.Configuration.Bind(settings));
    })
    .Build();

await host.RunAsync();
