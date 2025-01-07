//using Invest.Services.Quotes;

//var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddHostedService<Worker>();

//var host = builder.Build();
//host.Run();


using Dapper;
using HelgiLab.Extentions.Linq;
using Npgsql;
using System.Globalization;

var source = "C:\\BackUp\\Quotes\\MOEX_SBER\\MOEX_SBER_1_20200101_20250105.csv";
var connectionString = "Host=frog;Database=quotes;Username=postgres;Password=postgre;";
var BATCH_SIZE = 100000;

IFormatProvider provider = CultureInfo.InvariantCulture;

var quoteBatches = File.ReadLines(source)
    .Select(x => x.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
    .SkipWhile(x => x[0].Equals("<TICKER>", StringComparison.OrdinalIgnoreCase))
    .Select(x => new { 
        Ticker = x[0], 
        Period = int.Parse(x[1]),
        Dttm = DateTimeOffset.ParseExact(x[2] + x[3], "yyyyMMddHHmmss", provider).ToUniversalTime(),
        Open = float.Parse(x[4], provider),
        High = float.Parse(x[5], provider),
        Low = float.Parse(x[6], provider),
        Close = float.Parse(x[7], provider),
        Volume = int.Parse(x[8], provider),
    })
    .SplitIf(list => list.Count == BATCH_SIZE)
    ;

const string sql = @"INSERT INTO
    moex.sber_1 (dttm, open, high, low, close, volume) 
    VALUES (@Dttm, @Open, @High, @Low, @Close, @Volume)";

var rowsAffected = 0;
foreach (var batch in quoteBatches)
{
    using var connection = new NpgsqlConnection(connectionString);
    connection.Open();

    using var transaction = connection.BeginTransaction();
    rowsAffected += connection.Execute(sql, batch, transaction: transaction);
    transaction.Commit();
    Console.WriteLine($"{rowsAffected} row(s) affected.");
}