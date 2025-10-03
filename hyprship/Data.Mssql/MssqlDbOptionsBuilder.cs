using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Hyprship.Data.Mssql;

public class MssqlDbOptionsBuilder : DbContextOptionsBuilder
{
    public MssqlDbOptionsBuilder(
        string? connectionString = null,
        Action<SqlServerDbContextOptionsBuilder>? configure = null)
    {
        ApplyDefaults(this, connectionString, configure);
    }

    public static void ApplyDefaults(
        DbContextOptionsBuilder optionsBuilder,
        string? connectionString = null,
        Action<SqlServerDbContextOptionsBuilder>? configure = null)
    {
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.UseSnakeCaseNamingConvention();

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            connectionString = Environment.GetEnvironmentVariable("HYPRSHIP_MSSQL_CONNECTION_STRING")
                               ?? Environment.GetEnvironmentVariable("HYPRSHIP_MSSQL_CONNSTR");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                var host = Environment.GetEnvironmentVariable("HYPRSHIP_MSSQL_HOST") ??
                           "localhost";

                var port = Environment.GetEnvironmentVariable("HYPRSHIP_MSSQL_PORT") ??
                           "1433";

                var db = Environment.GetEnvironmentVariable("HYPRSHIP_MSSQL_DB") ??
                         "hyprship";

                var user = Environment.GetEnvironmentVariable("HYPRSHIP_MSSQL_USER") ??
                           "sa";

                var password = Environment.GetEnvironmentVariable("HYPRSHIP_MSSQL_PASSWORD") ??
                               "Your_password123";

                var encrypt = Environment.GetEnvironmentVariable("HYPRSHIP_MSSQL_ENCRYPT") ?? "false";

                var trustServerCertificate =
                    Environment.GetEnvironmentVariable("HYPRSHIP_MSSQL_TRUST_SERVER_CERTIFICATE") ?? "true";

                connectionString =
                    $"Server={host},{port};Database={db};User Id={user};Password={password};MultipleActiveResultSets=true; Encrypt={encrypt};";
            }
        }

        configure ??= (sqlOptions) =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
            sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            sqlOptions.MigrationsHistoryTable("migration_history");

            int commandTimeout = 60;
            var ct = Environment.GetEnvironmentVariable("HYPRSHIP_MSSQL_COMMAND_TIMEOUT");
            if (int.TryParse(ct, out var parsed) && parsed > 0)
                commandTimeout = parsed;

            sqlOptions.CommandTimeout(commandTimeout);
        };

        optionsBuilder.UseSqlServer(connectionString, configure);
    }
}