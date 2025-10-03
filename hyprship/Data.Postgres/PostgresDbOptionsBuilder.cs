using Hyprship.Data.Models;

using Microsoft.EntityFrameworkCore;

using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace Hyprship.Data.Postgres;

public class PostgresDbOptionsBuilder : DbContextOptionsBuilder<Db>
{
    public PostgresDbOptionsBuilder(
        string? connectionString = null,
        Action<NpgsqlDbContextOptionsBuilder>? configure = null)
        : base()
    {
        ApplyDefaults(this, connectionString, configure);
    }

    public static void ApplyDefaults(
        DbContextOptionsBuilder builder,
        string? connectionString = null,
        Action<NpgsqlDbContextOptionsBuilder>? configure = null)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString ??= Environment.GetEnvironmentVariable("HYPRSHIP_POSTGRES_CONNECTION_STRING")
                                 ?? Environment.GetEnvironmentVariable("HYPRSHIP_POSTGRES_CONNSTR")
                                 ?? Environment.GetEnvironmentVariable("HYPRSHIP_PG_CONNSTR");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                var host = Environment.GetEnvironmentVariable("HYPRSHIP_POSTGRES_HOST") ??
                           Environment.GetEnvironmentVariable("HYPRSHIP_POSTGRES_HOST") ??
                           Environment.GetEnvironmentVariable("HYPRSHIP_PG_HOST");

                var port = Environment.GetEnvironmentVariable("HYPRSHIP_POSTGRES_PORT") ??
                           Environment.GetEnvironmentVariable("HYPRSHIP_POSTGRES_PORT") ??
                           Environment.GetEnvironmentVariable("HYPRSHIP_PG_PORT") ?? "5432";

                var db = Environment.GetEnvironmentVariable("HYPRSHIP_POSTGRES_DB") ??
                         Environment.GetEnvironmentVariable("HYPRSHIP_POSTGRES_DB") ??
                         Environment.GetEnvironmentVariable("HYPRSHIP_PG_DB") ?? "hyprship";

                var user = Environment.GetEnvironmentVariable("HYPRSHIP_POSTGRES_USER") ??
                           Environment.GetEnvironmentVariable("HYPRSHIP_POSTGRES_USER") ??
                           Environment.GetEnvironmentVariable("HYPRSHIP_PG_USER") ?? "postgres";

                var password = Environment.GetEnvironmentVariable("HYPRSHIP_POSTGRES_PASSWORD") ??
                               Environment.GetEnvironmentVariable("HYPRSHIP_POSTGRES_PASSWORD") ??
                               Environment.GetEnvironmentVariable("HYPRSHIP_PG_PASSWORD") ?? "your_password";

                connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={password}";
            }
        }

        configure ??= (options) =>
        {
            options.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorCodesToAdd: null);

            options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            options.MigrationsHistoryTable("migration_history", "public");

            int commandTimeout = 60;
            var ct = Environment.GetEnvironmentVariable("HYPRSHIP_MYSQL_COMMAND_TIMEOUT");
            if (int.TryParse(ct, out var parsed) && parsed > 0)
                commandTimeout = parsed;

            options.CommandTimeout(commandTimeout);
        };

        builder.UseNpgsql(connectionString, configure);
        builder.UseSnakeCaseNamingConvention();
    }
}