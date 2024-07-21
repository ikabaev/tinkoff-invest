using Microsoft.Extensions.Configuration;

namespace Testing.StockSharp.App
{
    internal class AppSettings
    {
        public IConfigurationSection[] Providers { get; set; } = [];
    }
}
