// See https://aka.ms/new-console-template for more information

using Sequent.FileLoader.Client;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact.Reader;

const string logsPath = "/Users/asim-work/logs/main/alt202404130100.txt";


IList<LogEvent> LoadEvents()
{
    using var reader = new LogEventReader(File.OpenText(logsPath));
    IList<LogEvent> logEvents = new List<LogEvent>();
    LogEvent evt;
    while (reader.TryRead(out evt))
    {
        logEvents.Add(evt);
    }

    return logEvents;
}

void LogEvents()
{
    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.Seq("http://localhost:5341")
        .CreateLogger();

    foreach (var @event in LoadEvents())
    {
        Log.Logger.Write(@event);
    }

    Log.CloseAndFlush();
}

await UserLogProducer.LogUsers();
