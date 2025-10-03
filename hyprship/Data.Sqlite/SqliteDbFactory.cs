using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Hyprship.Data.Sqlite;

public class SqliteDbFactory : IDesignTimeDbContextFactory<SqliteDb>
{
    public SqliteDb CreateDbContext(string[] args)
    {
        string? connectionString = null;
        for (var i = 0; i < args.Length; i++)
        {
            if (args[i] == "--connection-string" && i + 1 < args.Length)
            {
                connectionString = args[i + 1];
                break;
            }
        }

        var optionsBuilder = new SqliteDbOptionsBuilder(connectionString);
        return new SqliteDb(optionsBuilder.Options);
    }
}