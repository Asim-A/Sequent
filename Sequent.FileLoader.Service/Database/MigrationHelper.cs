// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Sequent.Models;

namespace Sequent.FileLoader.Service.Database;

public static class MigrationHelper
{
    public static void MigrateAndConfigure(this TrackerContext db, bool isDevelopment = false)
    {
        db.Database.EnsureCreated();
        db.Database.Migrate();

        if (!isDevelopment) return;
        
        SetupDefaultDevelopmentTrackingDirectory(db);
    }

    private static void SetupDefaultDevelopmentTrackingDirectory(TrackerContext db)
    {
        var exists = db.TrackingDirectories.Any(t => t.Path == "/Users/asim-work/logs");
        if (exists) return;

        var defaultDirectory = new TrackingDirectory { Added = DateTime.UtcNow, Path = "/Users/asim-work/logs" };
        db.TrackingDirectories.Add(defaultDirectory);
        db.SaveChanges();
    }
}
