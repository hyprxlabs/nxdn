using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Hyprship.Data.Postgres;

public class PostgresDbDbFactory : IDesignTimeDbContextFactory<PostgresDb>
{
    public PostgresDb CreateDbContext(string[] args)
    {
        string? connectionString = null;
        for(var i = 0; i < args.Length; i++)
        {
            if (args[i] == "--connection-string" && i + 1 < args.Length)
            {
                connectionString = args[i + 1];
                break;
            }
        }

        var optionsBuilder = new PostgresDbOptionsBuilder(connectionString);
        return new PostgresDb(optionsBuilder.Options);
    }
}