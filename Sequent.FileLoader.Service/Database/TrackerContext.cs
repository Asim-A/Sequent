using Microsoft.EntityFrameworkCore;
using Sequent.Models;
using Env = System.Environment;
using FileInfo = Sequent.Models.FileInfo;

namespace Sequent.FileLoader.Service.Database;

public class TrackerContext : DbContext
{
    public TrackerContext()
    {
        var path = Env.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        DbPath = Path.Join(path, "sequent.db");
        Console.WriteLine($"Special path: {DbPath}");
    }

    public DbSet<TrackingDirectory> TrackingDirectories { get; set; }
    public DbSet<FileInfo> FileInfos { get; set; }
    public string DbPath { get; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }
}
