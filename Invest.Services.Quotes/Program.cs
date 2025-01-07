//using Invest.Services.Quotes;

//var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddHostedService<Worker>();

//var host = builder.Build();
//host.Run();


using Dapper;
using HelgiLab.Extentions.Linq;
using Npgsql;
using System.Data;
using System.Globalization;

var source = "C:\\BackUp\\Quotes\\MOEX_SBER\\MOEX_SBER_1_20200101_20250105.csv";
var connectionString = "Host=frog;Database=quotes;Username=postgres;Password=postgre;";
var BATCH_SIZE = 100000;

IFormatProvider provider = CultureInfo.InvariantCulture;





//var quoteBatches = File.ReadLines(source)
//    .Select(x => x.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
//    .SkipWhile(x => x[0].Equals("<TICKER>", StringComparison.OrdinalIgnoreCase))
//    .Select(x => new { 
//        Ticker = x[0], 
//        Period = int.Parse(x[1]),
//        Dttm = DateTimeOffset.ParseExact(x[2] + x[3], "yyyyMMddHHmmss", provider).ToUniversalTime(),
//        Open = float.Parse(x[4], provider),
//        High = float.Parse(x[5], provider),
//        Low = float.Parse(x[6], provider),
//        Close = float.Parse(x[7], provider),
//        Volume = int.Parse(x[8], provider),
//    })
//    .SplitIf(list => list.Count == BATCH_SIZE)
//    ;

//const string sql = @"INSERT INTO
//    moex.sber_1 (dttm, open, high, low, close, volume) 
//    VALUES (@Dttm, @Open, @High, @Low, @Close, @Volume)";

var rowsAffected = 0;
//foreach (var batch in quoteBatches)
//{
//    using var connection = new NpgsqlConnection(connectionString);
//    connection.Open();

//    using var transaction = connection.BeginTransaction();
//    rowsAffected += connection.Execute(sql, batch, transaction: transaction);
//    transaction.Commit();
//    Console.WriteLine($"{rowsAffected} row(s) affected.");
//}


const string sql = @"select dttm, open, high, low, close, volume from moex.sber_1 order by dttm";
using var conn_read = new NpgsqlConnection(connectionString);
conn_read.Open();

IDataReader reader = conn_read.ExecuteReader(sql);
var quoteBricks = reader
    .SeqWhile(r => r.Read())
    .Select(r => new
    {
        Dttm = r.GetDateTime(0),
        Open = r.GetFloat(1),
        High = r.GetFloat(2),
        Low = r.GetFloat(3),
        Close = r.GetFloat(4),
        Volume = r.GetInt32(5),
    })
    .Select(q => new {
        q.Dttm,
        Value = (q.High + q.Low) / 2,
        Spread = (q.High - q.Low) / 2,
        q.Volume
    })
    .SplitIf(list => list.Max(q => q.Value) / list.Min(q => q.Value) - 1 >= .01)
    .Select((b, i) => new {
        Id = i,
        b.Begin,
        b.End,
        Max = b.Max(q => q.Value),
        Min = b.Min(q => q.Value),
        Volume = b.Sum(q => q.Volume),
        b.Count
    })
    .Windowed(2)
    .Select(w => new { 
        w.Begin.Id,
        Begin = w.Begin.Begin.Dttm,
        End = w.Begin.End.Dttm,
        // направление выхода 
        Direction = (w.End.Max / w.Begin.Max - 1) * 100,
        w.Begin.Volume
    })
    .SplitIf(list => list.Count == 1000)
    ;


const string sql_bricks = @"INSERT INTO
    moex.sber_1_brick_1 (id, dttm_begin, dttm_end, direction, volume) 
    VALUES (@Id, @Begin, @End, @Direction, @Volume)";

foreach (var bricks in quoteBricks)
{
    using var connection = new NpgsqlConnection(connectionString);
    connection.Open();

    using var transaction = connection.BeginTransaction();
    rowsAffected += connection.Execute(sql_bricks, bricks, transaction: transaction);
    transaction.Commit();
    Console.WriteLine($"{rowsAffected} row(s) affected.");
}