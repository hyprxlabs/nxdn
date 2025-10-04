using Hyprx.AspNetCore.Identity;
using Hyprx.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Hyprship.Data.Models;

public partial class Db : DbContext
{
    public Db(DbContextOptions options)
        : base(options)
    {
        Console.WriteLine("Db constructor called");
    }

    public DbSet<User> Users { get; set; } = default!;

    public DbSet<UserPasswordAuth> UserPasswordAuths { get; set; } = default!;

    public DbSet<UserClaim> UserClaims { get; set; } = default!;

    public DbSet<UserLoginProvider> UserLoginProviders { get; set; } = default!;

    public DbSet<UserLoginProviderToken> UserLoginProviderTokens { get; set; } = default!;

    public DbSet<UserPasskey> UserPasskeys { get; set; } = default!;

    public DbSet<UserApiKey> UserApiKeys { get; set; } = default!;

    public DbSet<Role> Roles { get; set; } = default!;

    public DbSet<RoleClaim> RoleClaims { get; set; } = default!;

    public DbSet<Group> Groups { get; set; } = default!;

    public DbSet<GroupClaim> GroupClaims { get; set; } = default!;
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Console.WriteLine("OnConfiguring");
        if (optionsBuilder.IsConfigured)
            return;

        Console.WriteLine("OnConfiguring invoking...");
        optionsBuilder.UseSnakeCaseNamingConvention();
    }

    protected StoreOptions? GetStoreOptions() => this.GetService<IDbContextOptions>()
        .Extensions.OfType<CoreOptionsExtension>()
        .FirstOrDefault()?.ApplicationServiceProvider
        ?.GetService<IOptions<IdentityOptions>>()
        ?.Value?.Stores;

    protected sealed class PersonalDataConverter : ValueConverter<string, string>
    {
        public PersonalDataConverter(IPersonalDataProtector protector)
            : base(s => protector.Protect(s), s => protector.Unprotect(s), default)
        {
        }
    }
}