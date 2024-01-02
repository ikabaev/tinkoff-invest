using Invest.Services;
using Tinkoff.InvestApi;

namespace Invest.Services
{
    static public class TinkoffInvestAPIServiceExtention
    {
        public static string accessToke { get; set; }
        public static TinkoffInvestHistoryClient HistoryData(this InvestApiClient service) => new(accessToke);
    }
}
