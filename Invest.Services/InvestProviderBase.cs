using Invest.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Invest.Services
{
    /// <summary>
    /// Базовый класс состояния счетов, позиций
    /// </summary>
    public abstract class InvestProviderBase(IConfigurationSection settings): IInvestProvider
    {
        readonly protected IConfigurationSection _settings = settings;
        public ICollection<Account> Accounts { get; } = new List<Account>();
    }
}
