using Microsoft.Extensions.Configuration;

namespace Invest.App
{
    internal class AppSettings
    {
        public IConfigurationSection[] Providers { get; set; } = [];
    }
}
