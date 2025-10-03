using Hyprship.Data.Models;

using Microsoft.EntityFrameworkCore;

namespace Hyprship.Data.Postgres;

public class PostgresDb : Db
{
    public PostgresDb(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
            return;

        PostgresDbOptionsBuilder.ApplyDefaults(optionsBuilder);
    }
}