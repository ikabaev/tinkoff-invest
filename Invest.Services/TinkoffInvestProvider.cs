using Microsoft.Extensions.Configuration;
using Tinkoff.InvestApi;

namespace Invest.Services
{
    public class TinkoffInvestProvider: InvestProviderBase
    {
        public TinkoffInvestProvider(IConfigurationSection settings) : base(settings)
        {
            var conf = new InvestApiSettings();
            settings.Bind(conf);
            this.Tinkoff = conf;
        }
        public InvestApiSettings Tinkoff { get;}
    }
}
