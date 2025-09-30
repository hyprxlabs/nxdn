using Hyprx.AspNetCore.Identity;
using Hyprx.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Hyprship.Data.Models;

public class Db : IdentityDbContext<User, Role, Guid>
{
    public Db(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<UserApiKey> UserApiKeys { get; set; } = null!;

    public DbSet<UserApiKeyRole> UserApiKeyRoles { get; set; } = null!;

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
                    FormattedName = "Admin",
                    ConcurrencyStamp = Guid.NewGuid().ToString("N"),
                };
                ctx.Set<Role>().Add(adminRole);
            }

            var rootServiceAccount = await ctx.Set<User>().FirstOrDefaultAsync(u => u.UserName == "hyprship_root", ct);
            if (rootServiceAccount is null)
            {
                var rootEmail = Environment.GetEnvironmentVariable("HYPRSHIP_ROOT_EMAIL");
                if (string.IsNullOrWhiteSpace(rootEmail))
                    rootEmail = "root@localhost";

                rootServiceAccount = new User
                {
                    Id = Guid.CreateVersion7(),
                    UserName = "hyprship_root",
                    FormattedUserName = "hyprship_root",
                    Email = rootEmail,
                    FormattedEmail = rootEmail,
                    SecurityStamp = Guid.NewGuid().ToString("N"),
                    ConcurrencyStamp = Guid.NewGuid().ToString("N"),
                };

                ctx.Set<User>().Add(rootServiceAccount);
            }
        });

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder builder)
    {
        // Differences from Version 2:
        // - Add a passkey entity
        var storeOptions = this.GetStoreOptions();
        var maxKeyLength = storeOptions?.MaxLengthForKeys ?? 0;
        if (maxKeyLength == 0)
        {
            maxKeyLength = 128;
        }

        var encryptPersonalData = storeOptions?.ProtectPersonalData ?? false;
        PersonalDataConverter? converter = null;

        builder.Entity<User>(b =>
        {
            b.HasKey(u => u.Id);
            b.HasIndex(u => u.UserName).HasDatabaseName("ix_username").IsUnique();
            b.HasIndex(u => u.Email).HasDatabaseName("ix_email").IsUnique();
            b.ToTable("users");
            b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

            b.Property(u => u.FormattedUserName).HasMaxLength(256);
            b.Property(u => u.UserName).HasMaxLength(256);
            b.Property(u => u.FormattedEmail).HasMaxLength(256);
            b.Property(u => u.Email).HasMaxLength(256);

            if (encryptPersonalData)
            {
                converter = new PersonalDataConverter(this.GetService<IPersonalDataProtector>());
                var personalDataProps = typeof(User).GetProperties().Where(
                                prop => Attribute.IsDefined(prop, typeof(ProtectedPersonalDataAttribute)));
                foreach (var p in personalDataProps)
                {
                    if (p.PropertyType != typeof(string))
                    {
                        throw new InvalidOperationException("Only string properties can be protected.");
                    }

                    b.Property(typeof(string), p.Name).HasConversion(converter);
                }
            }

            b.HasMany<IdentityUserClaim<Guid>>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
            b.HasMany<IdentityUserLoginProvider<Guid>>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
            b.HasMany<IdentityUserToken<Guid>>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
            b.HasOne<IdentityUserPasswordAuth<Guid>>().WithOne().HasForeignKey<IdentityUserPasswordAuth<Guid>>(ua => ua.UserId).IsRequired();
            b.HasMany<IdentityUserPasskey<Guid>>().WithOne().HasForeignKey(up => up.UserId).IsRequired();
            b.HasMany<UserApiKey>(u => u.ApiKeys).WithOne(u => u.User).IsRequired();
        });

        builder.Entity<IdentityUserPasswordAuth<Guid>>(b =>
        {
            b.HasKey(ua => ua.UserId);
            b.Property(u => u.PhoneNumber).HasMaxLength(256);
            b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

            b.ToTable("user_password_auth");
            if (encryptPersonalData)
            {
                converter = new PersonalDataConverter(this.GetService<IPersonalDataProtector>());
                var personalDataProps = typeof(IdentityUserPasswordAuth).GetProperties().Where(
                                prop => Attribute.IsDefined(prop, typeof(ProtectedPersonalDataAttribute)));
                foreach (var p in personalDataProps)
                {
                    if (p.PropertyType != typeof(string))
                    {
                        throw new InvalidOperationException("Only string properties can be protected.");
                    }

                    b.Property(typeof(string), p.Name).HasConversion(converter);
                }
            }
        });

        builder.Entity<IdentityUserClaim<Guid>>(b =>
        {
            b.HasKey(uc => uc.Id);
            b.ToTable("user_claims");
        });

        builder.Entity<IdentityUserLoginProvider<Guid>>(b =>
        {
            b.HasKey(l => new { l.LoginProvider, l.ProviderKey });

            if (maxKeyLength > 0)
            {
                b.Property(l => l.LoginProvider).HasMaxLength(maxKeyLength);
                b.Property(l => l.ProviderKey).HasMaxLength(maxKeyLength);
            }

            b.ToTable("user_login_providers");
        });

        builder.Entity<IdentityUserToken<Guid>>(b =>
        {
            b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            if (maxKeyLength > 0)
            {
                b.Property(t => t.LoginProvider).HasMaxLength(maxKeyLength);
                b.Property(t => t.Name).HasMaxLength(maxKeyLength);
            }

            if (encryptPersonalData)
            {
                var tokenProps = typeof(IdentityUserToken<Guid>).GetProperties().Where(
                                prop => Attribute.IsDefined(prop, typeof(ProtectedPersonalDataAttribute)));
                foreach (var p in tokenProps)
                {
                    if (p.PropertyType != typeof(string))
                    {
                        throw new InvalidOperationException("Only string properties can be protected.");
                    }

                    b.Property(typeof(string), p.Name).HasConversion(converter);
                }
            }

            b.ToTable("user_tokens");
        });

        builder.Entity<IdentityUserPasskey<Guid>>(b =>
        {
            b.HasKey(p => p.CredentialId);
            b.ToTable("user_passkeys");
            b.Property(p => p.CredentialId).HasMaxLength(1024); // Defined in WebAuthn spec to be no longer than 1023 bytes
            b.OwnsOne(p => p.Data).ToJson();
        });

        // Currently no differences between Version 3 and Version 2
        builder.Entity<User>(b =>
        {
            b.HasMany<IdentityUserRole<Guid>>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
        });

        builder.Entity<Role>(b =>
        {
            b.HasKey(r => r.Id);
            b.HasIndex(r => r.Name).HasDatabaseName("ix_role_name").IsUnique();
            b.ToTable("roles");
            b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

            b.Property(u => u.FormattedName).HasMaxLength(256);
            b.Property(u => u.Name).HasMaxLength(256);

            b.HasMany<IdentityUserRole<Guid>>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
            b.HasMany<IdentityRoleClaim<Guid>>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
            b.HasMany<UserApiKeyRole>().WithOne(kr => kr.Role).HasForeignKey(kr => kr.RoleId).IsRequired();
        });

        builder.Entity<IdentityRoleClaim<Guid>>(b =>
        {
            b.HasKey(rc => rc.Id);
            b.ToTable("role_claims");
        });

        builder.Entity<IdentityUserRole<Guid>>(b =>
        {
            b.HasKey(r => new { r.UserId, r.RoleId });
            b.ToTable("user_roles");
        });

        builder.Entity<UserApiKey>(b =>
        {
            b.HasKey(k => k.Id);
            b.ToTable("user_api_keys");
            b.Property(k => k.Name).HasMaxLength(256);
            b.Property(k => k.KeyDigest).HasMaxLength(512);
            b.Property(k => k.CreatedAt).IsRequired();
            b.HasIndex(k => new { k.UserId, k.Name }).IsUnique();
            b.HasMany<UserApiKeyRole>(k => k.Roles).WithOne(kr => kr.UserApiKey).IsRequired();
        });
    }
}