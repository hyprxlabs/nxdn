using Hyprx.AspNetCore.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Hyprship.Data.Models;

public partial class Db
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        Console.WriteLine("OnModelCreating");

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
            b.HasIndex(u => u.UserName).HasDatabaseName("ix_users_upcase_user_name").IsUnique();
            b.HasIndex(u => u.Email).HasDatabaseName("ix_users_upcase_email").IsUnique();
            b.ToTable("users");
            b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

            b.Property(u => u.UpcaseUserName).HasMaxLength(256);
            b.Property(u => u.UserName).HasMaxLength(256);
            b.Property(u => u.UpcaseEmail).HasMaxLength(256);
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

            b.HasMany<UserRole>(u => u.UserRoles).WithOne(ur => ur.User).HasForeignKey(ur => ur.UserId).IsRequired();
            b.HasMany<UserClaim>(o => o.Claims).WithOne(o => o.User).HasForeignKey(uc => uc.UserId).IsRequired();
            b.HasMany<UserLoginProvider>(o => o.LoginProviders).WithOne(lp => lp.User).HasForeignKey(ul => ul.UserId).IsRequired();
            b.HasMany<UserToken>(u => u.Tokens).WithOne(o => o.User).HasForeignKey(ut => ut.UserId).IsRequired();
            b.HasOne<UserPasswordAuth>(u => u.PasswordAuth).WithOne(o => o.User).IsRequired();
            b.HasMany<UserPasskey>(u => u.Passkeys).WithOne(p => p.User).HasForeignKey(up => up.UserId).IsRequired();
            b.HasMany<UserApiKey>(u => u.ApiKeys).WithOne(u => u.User).IsRequired();
        });

        Console.WriteLine("OnModelCreating2");

        builder.Entity<UserPasswordAuth>(b =>
        {
            b.HasKey(ua => ua.UserId);
            b.Property(u => u.PhoneNumber).HasMaxLength(256);
            b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

            b.ToTable("user_password_auth");
            if (encryptPersonalData)
            {
                converter = new PersonalDataConverter(this.GetService<IPersonalDataProtector>());
                var personalDataProps = typeof(UserPasswordAuth).GetProperties().Where(
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

        Console.WriteLine("OnModelCreating3");

        builder.Entity<UserClaim>(b =>
        {
            b.HasKey(uc => uc.Id);
            b.ToTable("user_claims");
        });

        Console.WriteLine("OnModelCreating4");

        builder.Entity<UserLoginProvider>(b =>
        {
            b.HasKey(l => new { l.LoginProvider, l.ProviderKey });

            if (maxKeyLength > 0)
            {
                b.Property(l => l.LoginProvider).HasMaxLength(maxKeyLength);
                b.Property(l => l.ProviderKey).HasMaxLength(maxKeyLength);
            }

            b.ToTable("user_login_providers");
        });

        Console.WriteLine("OnModelCreating5");
        builder.Entity<UserToken>(b =>
        {
            b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            if (maxKeyLength > 0)
            {
                b.Property(t => t.LoginProvider).HasMaxLength(maxKeyLength);
                b.Property(t => t.Name).HasMaxLength(maxKeyLength);
            }

            if (encryptPersonalData)
            {
                var tokenProps = typeof(UserToken).GetProperties().Where(
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

        Console.WriteLine("OnModelCreating6");

        builder.Entity<UserPasskey>(b =>
        {
            b.HasKey(p => p.CredentialId);
            b.ToTable("user_passkeys");
            b.Property(p => p.CredentialId).HasMaxLength(1024); // Defined in WebAuthn spec to be no longer than 1023 bytes
            b.OwnsOne(p => p.Data, nb => nb.ToJson());
        });

        Console.WriteLine("OnModelCreating7");

        // Currently no differences between Version 3 and Version 2

        Console.WriteLine("OnModelCreating5");

        builder.Entity<Role>(b =>
        {
            b.HasKey(r => r.Id);
            b.HasIndex(r => r.Name).HasDatabaseName("ix_role_name").IsUnique();
            b.ToTable("roles");
            b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

            b.Property(u => u.UpcaseName).HasMaxLength(256);
            b.Property(u => u.Name).HasMaxLength(256);

            b.HasMany<UserRole>(r => r.UserRoles).WithOne(ur => ur.Role).HasForeignKey(ur => ur.RoleId).IsRequired();
            b.HasMany<RoleClaim>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
        });

        Console.WriteLine("OnModelCreating6");

        builder.Entity<RoleClaim>(b =>
        {
            b.HasKey(rc => rc.Id);
            b.ToTable("role_claims");
        });

        Console.WriteLine("OnModelCreating7");
        builder.Entity<Group>(b =>
        {
            Console.WriteLine("Configuring Group entity");
            b.HasKey(g => g.Id);
            b.Property(o => o.Name).HasMaxLength(128);
            b.Property(o => o.UpcaseName).HasMaxLength(128);
            b.HasIndex(g => g.UpcaseName).HasDatabaseName("ix_groups_upcase_name").IsUnique();
            b.ToTable("groups");
            b.Property(g => g.ConcurrencyStamp).IsConcurrencyToken();

            Console.WriteLine("Setting up relationships for Group entity");
            b.HasMany(o => o.Roles)
                .WithMany(r => r.Groups)
                .UsingEntity("groups_roles");

            Console.WriteLine("Setting up Claims relationship for Group entity");
            b.HasMany(o => o.Claims)
                .WithOne(o => o.Group)
                .HasForeignKey(o => o.GroupId)
                .IsRequired();

            Console.WriteLine("Setting up Users relationship for Group entity");
            b.HasMany(o => o.Users)
                .WithMany(u => u.Groups)
                .UsingEntity("groups_users");

            Console.WriteLine("Setting up Admins relationship for Group entity");
            b.HasMany(o => o.Admins)
                .WithMany(o => o.Groups)
                .UsingEntity("groups_admins");

            Console.WriteLine("Finished configuring Group entity");
        });

        Console.WriteLine("OnModelCreating8");
        builder.Entity<GroupClaim>(b =>
        {
            b.HasKey(o => o.Id);
            b.Property(o => o.ClaimType).HasMaxLength(256);
            b.Property(o => o.ClaimValue);
            b.ToTable("group_claims");
            b.HasOne(o => o.Group).WithMany(g => g.Claims).HasForeignKey(o => o.GroupId).IsRequired();
        });

        Console.WriteLine("OnModelCreating9");
        builder.Entity<UserRole>(b =>
        {
            b.HasKey(r => new { r.UserId, r.RoleId });
            b.ToTable("user_roles");
        });

        Console.WriteLine("OnModelCreatin10");

        builder.Entity<UserApiKey>(b =>
        {
            b.HasKey(k => k.Id);
            b.ToTable("user_api_keys");
            b.Property(k => k.Name).HasMaxLength(256);
            b.Property(k => k.KeyDigest).HasMaxLength(512);
            b.Property(k => k.CreatedAt).IsRequired();
            b.HasIndex(k => new { k.UserId, k.Name }).IsUnique();
            b.HasMany(k => k.UserApiKeyRoles).WithOne(r => r.UserApiKey).HasForeignKey(r => r.UserApiKeyId).IsRequired();
        });

        builder.Entity<UserApiKeyRole>(b =>
        {
            b.HasKey(r => new { r.UserApiKeyId, r.RoleId });
            b.ToTable("user_api_keys_roles");
        });

        Console.WriteLine("OnModelCreating complete");
    }
}