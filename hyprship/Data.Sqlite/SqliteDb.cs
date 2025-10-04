using Hyprship.Data.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Hyprship.Data.Sqlite;

public class SqliteDb : Db
{
    public SqliteDb(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Console.WriteLine("OnConfiguring");
        if (optionsBuilder.IsConfigured)
            return;

        SqliteDbOptionsBuilder.ApplyDefaults(optionsBuilder);
    }
}