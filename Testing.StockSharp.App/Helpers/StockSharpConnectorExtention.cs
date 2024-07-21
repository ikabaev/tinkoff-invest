using Ecng.Common;
using Ecng.Configuration;
using Ecng.Serialization;
using StockSharp.Algo.Storages;
using StockSharp.Algo;
using StockSharp.BusinessEntities;
using StockSharp.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockSharp.Localization;

namespace Testing.StockSharp.App.Helpers
{
    internal  static class StockSharpConnectorExtention
    {
        public static Connector Init(this Connector connector)
        {
            // subscribe on connection successfully event
            connector.Connected += () =>
            {
                Console.WriteLine("Connected");
            };
            // subscribe on connection error event
            connector.ConnectionError += error => 
            {
                Console.WriteLine(error.ToString());
            };
            connector.Disconnected += () => Console.WriteLine("Disconnected"); 
            // subscribe on error event
            connector.Error += error => Console.WriteLine(error.ToString());
            // subscribe on error of market data subscription event
            connector.SubscriptionFailed += (security, msg, error) => Console.WriteLine(error.ToString());

            //connector.NewSecurity += _securitiesWindow.SecurityPicker.Securities.Add;
            //connector.NewTrade += _tradesWindow.TradeGrid.Trades.Add;
            //connector.NewOrder += _ordersWindow.OrderGrid.Orders.Add;
            //connector.NewStopOrder += _stopOrdersWindow.OrderGrid.Orders.Add;
            //connector.NewMyTrade += _myTradesWindow.TradeGrid.Trades.Add;

            //connector.PositionReceived += (sub, p) => _portfoliosWindow.PortfolioGrid.Positions.TryAdd(p);

            //// subscribe on error of order registration event
            //connector.OrderRegisterFailed += _ordersWindow.OrderGrid.AddRegistrationFail;
            //// subscribe on error of order cancelling event
            //connector.OrderCancelFailed += OrderFailed;
            //// subscribe on error of stop-order registration event
            //connector.OrderRegisterFailed += _stopOrdersWindow.OrderGrid.AddRegistrationFail;
            //// subscribe on error of stop-order cancelling event
            //connector.StopOrderCancelFailed += OrderFailed;
            //// set market data provider
            //_securitiesWindow.SecurityPicker.MarketDataProvider = connector;
            //try
            //{
            //    if (File.Exists(_settingsFile))
            //    {
            //        var ctx = new ContinueOnExceptionContext();
            //        ctx.Error += ex => ex.LogError();
            //        using (new Scope<ContinueOnExceptionContext>(ctx))
            //            connector.Load(new JsonSerializer<SettingsStorage>().Deserialize(_settingsFile));
            //    }
            //}
            //catch
            //{
            //}
            //ConfigManager.RegisterService<IExchangeInfoProvider>(new InMemoryExchangeInfoProvider());

            //// needed for graphical settings
            //ConfigManager.RegisterService<IMessageAdapterProvider>(new FullInMemoryMessageAdapterProvider(connector.Adapter.InnerAdapters));

            return connector;
        }

    }
}
