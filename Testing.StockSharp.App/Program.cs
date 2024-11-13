// See https://aka.ms/new-console-template for more information
using Ecng.Collections;
using Microsoft.Extensions.Configuration;
/*
 * Тестовое приложение для взаимодействия с функционалом одного типа
 * На примере взаимодействия с сервисами для биржевых операций
 * подключаем через di коннекторы - объекты для взаимодействия с биржвым сервисом
 * настраиваем получение инструментов с которыми работает коннектор (объекты работы с состояние)
 * настраиваем получении истории по инструментам (история состояний)
 * разрабатываем или подключаем алгоритмы работы с состояниями тактик, стратегии и оперативного управления и рисков
 * анализ эффективности алгоритмов с учетом издержек конкретного поставщика - комисий, временных и т.д.
 * тестируем в на исторических данных
 * настраиваем получении текущего состояния по инструментам (тики, сделки)
 * тестируем в реальном времени
 */
using Microsoft.Extensions.Hosting;
using StockSharp.Algo;
using StockSharp.Tinkoff;
using System.Security;
using Testing.StockSharp.App;
using Testing.StockSharp.App.Helpers;

var builder = Host.CreateDefaultBuilder(args);

var host = builder
    .ConfigureHostConfiguration(context => context.AddUserSecrets<Program>())
    .ConfigureServices((context, services) =>
    {
        var connector = new Connector()
        /// подключение всех событий
            .Init();

        var settings = context.Configuration.Get<AppSettings>() ?? new();
        foreach (var setting in settings.Providers)
        {
            TraderHelper
            /// подключение Тинькоф
                .AddAdapter<TinkoffMessageAdapter>(connector, adapter =>
                {
                    var accessToken = setting["AccessToken"]?.ToCharArray() ?? throw new SecurityException("AccessToken");
                    //unsafe
                    //{
                    //    fixed (char* pointerToFirst = accessToken)
                    //        adapter.Token = new SecureString(pointerToFirst, accessToken.Length);
                    //}

                    var securToken = new SecureString();
                    accessToken.ForEach(x => { securToken.AppendChar(x); });
                    adapter.Token = securToken;
                    

                });
        }
        connector.Connect();
    })
    .Build();


await host.RunAsync();