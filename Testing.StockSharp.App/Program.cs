// See https://aka.ms/new-console-template for more information
using Ecng.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using StockSharp.Algo;
using StockSharp.Tinkoff;
using System.Security;
using Testing.StockSharp.App;
using Testing.StockSharp.App.Helpers;

var builder = Host.CreateDefaultBuilder(args);

var host = builder
    .ConfigureHostConfiguration(context => context.AddUserSecrets<Program>())
    .ConfigureServices((context, services) =>
    {
        var connector = new Connector()
            .Init();

        var settings = context.Configuration.Get<AppSettings>() ?? new();
        foreach (var setting in settings.Providers)
        {
            TraderHelper
                .AddAdapter<TinkoffMessageAdapter>(connector, adapter =>
                {
                    var accessToken = setting["AccessToken"] ?? throw new ArgumentNullException("AccessToken");
                    adapter.Token = new SecureString();
                    accessToken.ForEach(x => { adapter.Token.AppendChar(x); });
                    
                });
        }
        connector.Connect();
    })
    .Build();


await host.RunAsync();