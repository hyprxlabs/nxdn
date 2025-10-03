using Hyprship.Data.Models;

using Microsoft.EntityFrameworkCore;

namespace Hyprship.Data.Mssql;

public class MssqlDb : Db
{
    public MssqlDb(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=hyprship;User Id=sa;Password=Your_password123;");
            optionsBuilder.UseSnakeCaseNamingConvention();
        }

        base.OnConfiguring(optionsBuilder);
    }
}