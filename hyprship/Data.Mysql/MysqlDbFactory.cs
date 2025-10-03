using Microsoft.EntityFrameworkCore.Design;

namespace Hyprship.Data.Mysql;

public class MysqlDbFactory : IDesignTimeDbContextFactory<MysqlDb>
{
    public MysqlDb CreateDbContext(string[] args)
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

        var optionsBuilder = new MysqlDbOptionsBuilder(connectionString);
        return new MysqlDb(optionsBuilder.Options);
    }
}