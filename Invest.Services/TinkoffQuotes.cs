using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invest.Services
{
    /// <summary>
    /// Сервис котировок. Подписка и трансляция
    /// </summary>
    internal class TinkoffQuotes(List<string> tikers)
    {
        private readonly List<string> _tikers = tikers;

        public void AddQuotes(string tiker) { _tikers.Append(tiker); }
        public IReadOnlyCollection<string> Tikers => _tikers;
    }
}
