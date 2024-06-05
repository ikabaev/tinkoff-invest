using Invest.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Tinkoff.InvestApi;

namespace Tinkoff.Invest.Services
{
    /// <summary>
    /// Предоставляет доступ к Tinkoff Invest
    /// </summary>
    public class TinkoffInvestProvider: IInvestProvider
    {
        private readonly InvestApiClient _client;
        /// <summary>
        /// Создание по конфигурация
        /// </summary>
        /// <param name="settings">параметры</param>
        public TinkoffInvestProvider(IConfigurationSection section)
        {
            if (section == null)
                throw new ArgumentException($"Отсутствует конфигурации для {nameof(TinkoffInvestProvider)}");
            
            // получаем из конфигурации приложения
            var apiSettings = section.Get<InvestApiSettings>();

            if (apiSettings == null)
                throw new ArgumentException($"Ошибки в конфигурации для {nameof(TinkoffInvestProvider)}");

            // создаем клиента
            _client = InvestApiClientFactory.Create(apiSettings);
        }
    }
}
