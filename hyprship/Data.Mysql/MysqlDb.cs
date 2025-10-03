using Hyprship.Data.Models;

using Microsoft.EntityFrameworkCore;

namespace Hyprship.Data.Mysql;

public class MysqlDb : Db
{
    public MysqlDb(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
            return;

        optionsBuilder.UseSnakeCaseNamingConvention();

        optionsBuilder.UseMySQL("server=localhost;database=hyprship;user=root;password=your_password;", mySqlOptions =>
        {
            mySqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
        });
    }
}
