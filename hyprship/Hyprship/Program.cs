using Hyprship;
using Hyprship.Data.Models;
using Hyprship.Database.Models;

using Microsoft.EntityFrameworkCore;

try
{
    var builder = WebApplication.CreateBuilder(new WebApplicationOptions
    {
        Args = args,
        ContentRootPath = "public",
    });

    builder.AddHyprship();

    var app = builder.Build();

    app.UseHyprship();


    var config = app.Services.GetService<IConfigurationRoot>();
    Console.WriteLine(config is null ? "config is null" : "config is not null");
    var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetService<Db>();
    if (db is null)
    {
        Console.WriteLine("db is null");
    }

    db.Database.EnsureCreated();
    var seeder = ActivatorUtilities.CreateInstance<DbSeeder>(scope.ServiceProvider);
    await seeder.SeedAsync(db);

    app.Run();
    Console.WriteLine("application complete");
    Environment.Exit(0);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    Console.WriteLine(ex.StackTrace);
    Environment.Exit(1);
}