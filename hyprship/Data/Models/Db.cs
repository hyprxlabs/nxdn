using Hyprx.AspNetCore.Identity;
using Hyprx.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Hyprship.Data.Models;

public partial class Db : IdentityDbContext<User, Role, Guid>
{
    public Db(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<UserApiKey> UserApiKeys { get; set; } = null!;

    public DbSet<UserApiKeyRole> UserApiKeyRoles { get; set; } = null!;

    public DbSet<Group> Groups { get; set; } = null!;

    public DbSet<GroupClaim> GroupClaims { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseAsyncSeeding(async (ctx, _, ct) =>
        {
            var adminRole = await ctx.Set<Role>().FirstOrDefaultAsync(r => r.Name == "admin", ct);
            if (adminRole is null)
            {
                adminRole = new Role
                {
                    Id = Guid.CreateVersion7(),
                    Name = "admin",
                    UpcaseName = "Admin",
                    ConcurrencyStamp = Guid.NewGuid().ToString("N"),
                };
                ctx.Set<Role>().Add(adminRole);
            }

            var email = Environment.GetEnvironmentVariable("HS_ADMIN_EMAIL") ?? "hsadmin@localhost";
            var username = Environment.GetEnvironmentVariable("HS_ADMIN_USERNAME") ?? "hsadmin";
            var password = Environment.GetEnvironmentVariable("HS_ADMIN_PASSWORD") ?? "ChangeM3!";
            var rootServiceAccount =
                await ctx.Set<User>().FirstOrDefaultAsync(u => u.Email == email.ToUpperInvariant(), ct);
            if (rootServiceAccount is null)
            {
                var rootEmail = Environment.GetEnvironmentVariable("HS_ADMIN_EMAIL");
                if (string.IsNullOrWhiteSpace(rootEmail))
                    rootEmail = "hsadmin@localhost";

                rootServiceAccount = new User
                {
                    Id = Guid.CreateVersion7(),
                    UserName = username,
                    UpcaseUserName = username.ToUpperInvariant(),
                    Email = rootEmail,
                    UpcaseEmail = rootEmail.ToUpperInvariant(),
                    SecurityStamp = Guid.NewGuid().ToString("N"),
                    ConcurrencyStamp = Guid.NewGuid().ToString("N"),
                };

                ctx.Set<User>().Add(rootServiceAccount);

                var pw = Environment.GetEnvironmentVariable("HYPRSHIP_ROOT_PASSWORD") ?? "ChangeM3!";
                var login = new UserPasswordAuth()
                {
                    IsTwoFactorEnabled = false,
                    IsEmailVerified = true,
                    PasswordDigest = new PasswordHasher().HashPassword(pw),
                    UserId = rootServiceAccount.Id,
                    ConcurrencyStamp = Guid.NewGuid().ToString("N"),
                };
            }
        });

        base.OnConfiguring(optionsBuilder);
    }
}