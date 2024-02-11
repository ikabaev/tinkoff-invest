using Microsoft.Extensions.Configuration;

namespace Invest.Services
{
    public static class ProviderFactory
    {
        public static InvestProviderBase Create(IConfigurationSection settings) 
        {
            var type = settings["type"];
            if (type == "TinkoffInvest")
                return new TinkoffInvestProvider(settings);
            else throw new ArgumentException($"type '{type}' not found");
        }
    }
}
