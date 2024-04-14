using Sequent.FileLoader.Service.Database;
using SequentFileInfo = Sequent.Models.FileInfo;
using SystemFileInfo = System.IO.FileInfo;

namespace Sequent.FileLoader.Service;

public class DirectoriesTracker(ILogger<DirectoriesTracker> logger, IServiceScopeFactory serviceScopeFactory)
    : BackgroundService
{
    public Task Ping(CancellationToken stoppingToken)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogDebug("Worker running at: {time}", DateTimeOffset.Now);
        }

        return Task.Delay(5000, stoppingToken);
    }

    private IList<string> FindNewFilePathsToTrack(TrackerContext db)
    {
        var trackingDirectories = db.TrackingDirectories.ToList();
        trackingDirectories.ForEach(t => logger.LogInformation("Tracking directories: {}", trackingDirectories));
        var dbFileInfoPaths = db.FileInfos.Select(v => v.Path).ToList();

        return trackingDirectories
            .SelectMany(t =>
                Directory.GetFiles(t.Path, "*.txt", SearchOption.AllDirectories))
            .Except(dbFileInfoPaths)
            .ToList();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Ping(stoppingToken);

            using var scope = serviceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TrackerContext>();

            logger.LogDebug("Attempting to find new file infos");
            var newFilePaths = FindNewFilePathsToTrack(db);
            if (!newFilePaths.Any())
            {
                logger.LogDebug("No new files to track");
            }

            var newDbFileInfos = NewDbFileInfos(newFilePaths);

            newDbFileInfos.ForEach(n =>
            {
                logger.LogDebug("New file info added for tracking, path: {fp}", n.Path);
            });

            await db.FileInfos.AddRangeAsync(newDbFileInfos, stoppingToken);
            await db.SaveChangesAsync(stoppingToken);
        }
    }

    private List<SequentFileInfo> NewDbFileInfos(IList<string> newFilePaths)
    {
        var newDbFileInfos = new List<SequentFileInfo>();
        foreach (var filePath in newFilePaths)
        {
            var fileInfo = new SystemFileInfo(filePath);
            if (!fileInfo.Exists)
            {
                logger.LogDebug("File {fp} does not exist and could not be tracked.", filePath);
                continue;
            }

            var newFileInfo = new SequentFileInfo { Path = filePath, LastModified = DateTime.MinValue };


            newDbFileInfos.Add(newFileInfo);
        }

        return newDbFileInfos;
    }
}
