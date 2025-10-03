using Microsoft.EntityFrameworkCore;

using MySql.EntityFrameworkCore.Infrastructure;

namespace Hyprship.Data.Mysql;

public class MysqlDbOptionsBuilder : DbContextOptionsBuilder
{
    public MysqlDbOptionsBuilder(string? connectionString = null, Action<MySQLDbContextOptionsBuilder>? configure = null)
    {
        ApplyDefaults(this, connectionString, configure);
    }

    public static void ApplyDefaults(
        DbContextOptionsBuilder optionsBuilder,
        string? connectionString = null,
        Action<MySQLDbContextOptionsBuilder>? configure = null)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            connectionString = Environment.GetEnvironmentVariable("HYPRSHIP_MYSQL_CONNECTION_STRING")
                               ?? Environment.GetEnvironmentVariable("HYPRSHIP_MYSQL_CONNSTR");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                var host = Environment.GetEnvironmentVariable("HYPRSHIP_MYSQL_HOST") ??
                           "localhost";

                var port = Environment.GetEnvironmentVariable("HYPRSHIP_MYSQL_PORT") ??
                           "3306";

                var db = Environment.GetEnvironmentVariable("HYPRSHIP_MYSQL_DB") ??
                         "hyprship";

                var user = Environment.GetEnvironmentVariable("HYPRSHIP_MYSQL_USER") ??
                           "root";

                var password = Environment.GetEnvironmentVariable("HYPRSHIP_MYSQL_PASSWORD") ??
                               "your_password";

                connectionString = $"server={host};port={port};database={db};user={user};password={password};";
            }
        }

        configure ??= (mySqlOptions) =>
        {
            mySqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);

            mySqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            mySqlOptions.MigrationsHistoryTable("migration_history");

            int commandTimeout = 60;
            var ct = Environment.GetEnvironmentVariable("HYPRSHIP_MYSQL_COMMAND_TIMEOUT");
            if (int.TryParse(ct, out var parsed) && parsed > 0)
                commandTimeout = parsed;

            mySqlOptions.CommandTimeout(commandTimeout);
        };

        optionsBuilder.UseMySQL(connectionString, configure);
    }
}