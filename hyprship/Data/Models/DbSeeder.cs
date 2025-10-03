using Microsoft.EntityFrameworkCore;

namespace Hyprship.Database.Models;

public class DbSeeder
{
    public DbSeeder()
    {
    }

    public Task SeedAsync(DbContext db, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}