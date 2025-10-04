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

        var store = this.GetStoreOptions();
        var encryptPersonalData = store?.ProtectPersonalData ?? false;
        var converter = encryptPersonalData ? new PersonalDataConverter(this.GetService<IPersonalDataProtector>()) : null;


        builder.Entity<UserPasswordAuth>(b =>
        {
            b.HasKey(o => o.UserId);
            b.Property(u => u.PhoneNumber).HasMaxLength(256);
            b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
            if (encryptPersonalData)
            {
                b.Property(u => u.PhoneNumber).HasConversion(converter!);
            }

            b.ToTable("user_password_auth");
        });

        builder.Entity<UserClaim>(b =>
        {
            b.HasKey(uc => uc.Id);
            b.Property(uc => uc.ClaimType).HasMaxLength(128);
            b.ToTable("user_claims");
        });

        builder.Entity<UserLoginProvider>(b =>
        {
            b.HasKey(l => new { l.LoginProvider, l.ProviderKey });
            b.Property(l => l.LoginProvider).HasMaxLength(128);
            b.Property(l => l.ProviderKey).HasMaxLength(128);
            b.ToTable("user_login_provider");
        });

        builder.Entity<UserLoginProviderToken>(b =>
        {
            b.HasKey(t => new { UserId = t.UserId, t.LoginProvider, t.Name });
            b.Property(t => t.LoginProvider).HasMaxLength(128);
            b.Property(t => t.Name).HasMaxLength(128);
            b.Property(t => t.Value).HasMaxLength(1024);

            if (encryptPersonalData)
            {
                b.Property(t => t.Value).HasConversion(converter!);
            }

            b.ToTable("user_login_provider_tokens");
        });

        builder.Entity<UserPasskey>(b =>
        {
            b.HasKey(p => p.CredentialId);
            b.ToTable("user_passkeys");
            b.Property(p => p.CredentialId)
                .HasMaxLength(1024); // Defined in WebAuthn spec to be no longer than 1023 bytes
            b.OwnsOne(p => p.Data, nb => nb.ToJson());
        });

        builder.Entity<UserApiKey>(b =>
        {
            b.HasKey(k => k.Id);
            b.ToTable("user_api_keys");
            b.Property(k => k.Name).HasMaxLength(128);
            b.Property(k => k.KeyDigest).HasMaxLength(1024);
            b.Property(k => k.CreatedAt).IsRequired();
            b.HasIndex(k => new { k.UserId, k.Name }).IsUnique();
        });

        builder.Entity<User>(b =>
        {
            b.HasKey(u => u.Id);
            b.HasIndex(u => u.UserName).HasDatabaseName("ix_users_upcase_user_name").IsUnique();
            b.HasIndex(u => u.Email).HasDatabaseName("ix_users_upcase_email").IsUnique();
            b.ToTable("users");
            b.Property(u => u.ConcurrencyStamp).HasMaxLength(38).IsConcurrencyToken();
            b.Property(u => u.SecurityStamp).HasMaxLength(38);
            b.Property(u => u.UpcaseUserName).HasMaxLength(256);
            b.Property(u => u.UserName).HasMaxLength(256);
            b.Property(u => u.UpcaseEmail).HasMaxLength(256);
            b.Property(u => u.Email).HasMaxLength(256);

            if (encryptPersonalData)
            {
                b.Property(u => u.Email).HasConversion(converter!);
                b.Property(u => u.UserName).HasConversion(converter!);
            }

            b.HasOne(o => o.PasswordAuth)
                .WithOne(o => o.User)
                .HasForeignKey<UserPasswordAuth>(o => o.UserId);

            b.HasMany(o => o.Claims)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);

            b.HasMany(o => o.LoginProviders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);

            b.HasMany(o => o.LoginProviderTokens)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);

            b.HasMany(u => u.Passkeys)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId);

            b.HasMany(o => o.ApiKeys)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId);

            b.HasMany(o => o.Roles)
                .WithMany(o => o.Users)
                .UsingEntity("users_roles");
        });


        builder.Entity<RoleClaim>(b =>
        {
            b.HasKey(rc => rc.Id);
            b.Property(rc => rc.ClaimType).HasMaxLength(128);
            b.ToTable("role_claims");
        });

        builder.Entity<Role>(b =>
        {
            b.HasKey(r => r.Id);
            b.HasIndex(r => r.Name).HasDatabaseName("ix_role_name").IsUnique();
            b.ToTable("roles");
            b.Property(r => r.ConcurrencyStamp).HasMaxLength(38).IsConcurrencyToken();

            b.Property(u => u.UpcaseName).HasMaxLength(256);
            b.Property(u => u.Name).HasMaxLength(256);

            b.HasMany(o => o.UserApiKeys)
                .WithMany(o => o.Roles)
                .UsingEntity("user_api_keys_roles");

            // b.HasMany<UserRole>(r => r.UserRoles).WithOne(ur => ur.Role).HasForeignKey(ur => ur.RoleId).IsRequired();
            b.HasMany(o => o.Claims).WithOne(o => o.Role).HasForeignKey(rc => rc.RoleId);
        });

        builder.Entity<GroupClaim>(b =>
        {
            b.HasKey(o => o.Id);
            b.Property(o => o.ClaimType).HasMaxLength(256);
            b.Property(o => o.ClaimValue);
            b.ToTable("group_claims");
        });

        builder.Entity<Group>(b =>
        {
            Console.WriteLine("Configuring Group entity");
            b.HasKey(g => g.Id);
            b.Property(o => o.Name).HasMaxLength(128);
            b.Property(o => o.UpcaseName).HasMaxLength(38);
            b.HasIndex(g => g.UpcaseName).HasDatabaseName("ix_groups_upcase_name").IsUnique();
            b.ToTable("groups");
            b.Property(g => g.ConcurrencyStamp).HasMaxLength(128).IsConcurrencyToken();

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

        /*
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
        */
    }
}