using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tinkoff.InvestApi;

namespace Invest.Services
{
    public class TinkoffInvestProviderOld: InvestProviderBase
    {
        public TinkoffInvestProviderOld(IConfigurationSection settings) : base(settings)
        {
            var conf = new InvestApiSettings();
            settings.Bind(conf);
            //this.Tinkoff = conf;

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddInvestApiClient((_, settings) =>
            {
                settings.AccessToken = conf.AccessToken;
            });
            var serviceProvider = serviceCollection.BuildServiceProvider();

            this.Api = serviceProvider.GetRequiredService<InvestApiClient>();
        }
        public InvestApiClient Api { get;}
    }
}