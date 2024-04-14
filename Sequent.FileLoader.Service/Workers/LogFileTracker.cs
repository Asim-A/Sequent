using Microsoft.EntityFrameworkCore;
using Sequent.FileLoader.Service.Database;

namespace Sequent.FileLoader.Service;

public class LogFileTracker(ILogger<LogFileTracker> logger, IServiceScopeFactory serviceScopeFactory)
    : BackgroundService
{
    private readonly ILogger<LogFileTracker> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TrackerContext>();
            var fileInfos = db.FileInfos.ToList();
            var _ = await db.FileInfos.AnyAsync(stoppingToken);
        }
    }
}
