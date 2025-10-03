using Hyprship.Data.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using SqliteDbContextOptionsBuilder2 = Microsoft.EntityFrameworkCore.Infrastructure.SqliteDbContextOptionsBuilder;

namespace Hyprship.Data.Sqlite;

public class SqliteDbOptionsBuilder : DbContextOptionsBuilder<Db>
{
    public SqliteDbOptionsBuilder(string? connectionString = null, Action<SqliteDbContextOptionsBuilder2>? configure = null)
        : base()
    {
        ApplyDefaults(this);
    }

    public static void ApplyDefaults(
        DbContextOptionsBuilder optionsBuilder,
        string? connectionString = null,
        Action<SqliteDbContextOptionsBuilder2>? configure = null)
    {
        if (optionsBuilder.IsConfigured)
            return;

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            connectionString = Environment.GetEnvironmentVariable("HYPRSHIP_SQLITE_CONNECTION_STRING")
                               ?? Environment.GetEnvironmentVariable("HYPRSHIP_SQLITE_CONNSTR");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                var db = Environment.GetEnvironmentVariable("HYPRSHIP_SQLITE_DB");
                connectionString = $"Data Source={db ?? "hyprship.db"}";
            }
        }

        configure ??= (opt) =>
        {
            opt.MigrationsHistoryTable("migration_history");
            opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

            int commandTimeout = 60;
            var ct = Environment.GetEnvironmentVariable("HYPRSHIP_MYSQL_COMMAND_TIMEOUT");
            if (int.TryParse(ct, out var parsed) && parsed > 0)
                commandTimeout = parsed;

            opt.CommandTimeout(commandTimeout);
        };

        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseSqlite(connectionString, configure);
    }
}