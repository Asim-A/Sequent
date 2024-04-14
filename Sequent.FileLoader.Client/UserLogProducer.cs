using System.Text.Json;
using Serilog;
using Serilog.Context;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;

namespace Sequent.FileLoader.Client;

public class UserLogProducer
{
    public static async Task LogUsers()
    {
        var randomResultsAmount = new Random().Next(5, 111);
        var randomUserUrl = $"https://randomuser.me/api/?results={randomResultsAmount}";
        var c = new HttpClient();

        async Task<IList<RandomUser>?> RandomUsers()
        {
            var res = await c.GetAsync(randomUserUrl);
            var stream = await res.Content.ReadAsStreamAsync();

            var usersResponse = await JsonSerializer.DeserializeAsync<RandomUserResponse>(stream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return usersResponse?.Results;
        }

        var g = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.File(new CompactJsonFormatter(), "/Users/asim-work/logs/wip/gen.txt",
                rollingInterval: RollingInterval.Minute)
            .WriteTo.Console(new JsonFormatter())
            .CreateLogger();

        var a = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.File(new CompactJsonFormatter(), "/Users/asim-work/logs/main/alt.txt",
                rollingInterval: RollingInterval.Minute)
            .WriteTo.Console(new JsonFormatter())
            .CreateLogger();

        var settings = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var res = await c.GetAsync(randomUserUrl);
        var stream = await res.Content.ReadAsStreamAsync();

        var usersG = await RandomUsers();
        var usersA = await RandomUsers();

        foreach (var user in usersG!)
        {
            using (LogContext.PushProperty("user", user, true))
            {
                g.Information("New User Registered");
            }
        }

        foreach (var user in usersA!)
        {
            using (LogContext.PushProperty("customer", user, true))
            {
                a.Information("User made a purchase");
            }
        }
    }
}
