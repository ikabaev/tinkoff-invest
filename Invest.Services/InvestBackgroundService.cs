using Grpc.Core;
using Invest.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Tinkoff.InvestApi.V1;

namespace Invest.Services
{
    public class InvestBackgroundService(ILogger<InvestBackgroundService> logger, IEnumerable<IInvestProvider> investApi) : BackgroundService
    {
        async protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogDebug("service {investApi} starting...", nameof(investApi));
            
            var invest = investApi.FirstOrDefault();

            //logger.LogDebug("invest {yes} получен", invest == null ? "не": "");
            Console.WriteLine("invest {0} получен", invest == null ? "не" : "");

            /*
            var stream = _invest.Api.MarketDataStream.MarketDataStream(cancellationToken: stoppingToken);
            // Отправляем запрос в стрим
            await stream.RequestStream.WriteAsync(new MarketDataRequest
            {
                SubscribeCandlesRequest = new SubscribeCandlesRequest
                {
                    Instruments =
                    {
                        new CandleInstrument
                        {
                            InstrumentId = "BBG004730N88",
                            Interval = SubscriptionInterval.OneMinute
                        }
                    },
                    SubscriptionAction = SubscriptionAction.Subscribe
                }
            }, stoppingToken);

            // Обрабатываем все приходящие из стрима ответы
            await foreach (var response in stream.ResponseStream.ReadAllAsync(stoppingToken))
            {
                Console.WriteLine(JsonSerializer.Serialize(response));
            }
            */

            await Task.CompletedTask;
        }
    }
}
