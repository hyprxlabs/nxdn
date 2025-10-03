using Microsoft.EntityFrameworkCore.Design;

namespace Hyprship.Data.Mssql;

public class MssqlDbFactory : IDesignTimeDbContextFactory<MssqlDb>
{
    public MssqlDb CreateDbContext(string[] args)
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

        var optionsBuilder = new MssqlDbOptionsBuilder(connectionString);
        return new MssqlDb(optionsBuilder.Options);
    }
}