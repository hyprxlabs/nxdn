using Hyprship.Data.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Hyprship.Data.Sqlite;

public class SqliteDb : Db
{
    public SqliteDb(DbContextOptions<SqliteDb> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=hyprship.db");
            optionsBuilder.UseSnakeCaseNamingConvention();
        }

        base.OnConfiguring(optionsBuilder);
    }
}

public class SqliteDbFactory : IDesignTimeDbContextFactory<SqliteDb>
{
    public SqliteDb CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SqliteDb>();
        optionsBuilder.UseSqlite("Data Source=hyprship.db");
        optionsBuilder.UseSnakeCaseNamingConvention();
        return new SqliteDb(optionsBuilder.Options);
    }
}