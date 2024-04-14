using Microsoft.EntityFrameworkCore;
using Sequent.FileLoader.Service;
using Sequent.FileLoader.Service.Components;
using Sequent.FileLoader.Service.Database;
using Sequent.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();
builder.Services.AddHostedService<DirectoriesTracker>();
builder.Services.AddDbContext<TrackerContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapGet("/api", () => new { Message = "Hello, World" });

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TrackerContext>();
    db.MigrateAndConfigure(app.Environment.IsDevelopment());
}

app.Run();
