using Invest.Services;
using Tinkoff.InvestApi;

namespace Invest.Services
{
    static public class TinkoffInvestAPIServiceExtention
    {
        public static string accessToken { get; set; }
        public static TinkoffInvestHistoryClient HistoryData(this InvestApiClient service) => new(accessToken);
    }
}
