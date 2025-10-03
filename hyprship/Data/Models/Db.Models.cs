using Hyprx.AspNetCore.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Hyprship.Data.Models;

public partial class Db
{
    protected override void OnModelCreating(ModelBuilder builder)
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

            b.HasMany<IdentityUserClaim<Guid>>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
            b.HasMany<IdentityUserLoginProvider<Guid>>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
            b.HasMany<IdentityUserToken<Guid>>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
            b.HasOne<IdentityUserPasswordAuth<Guid>>().WithOne().HasForeignKey<IdentityUserPasswordAuth<Guid>>(ua => ua.UserId).IsRequired();
            b.HasMany<IdentityUserPasskey<Guid>>().WithOne().HasForeignKey(up => up.UserId).IsRequired();
            b.HasMany<UserApiKey>(u => u.ApiKeys).WithOne(u => u.User).IsRequired(); ;
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

            b.Property(u => u.UpcaseName).HasMaxLength(256);
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

        builder.Entity<Group>(b =>
        {
            b.HasKey(g => g.Id);
            b.Property(o => o.Name).HasMaxLength(128);
            b.Property(o => o.UpcaseName).HasMaxLength(128);
            b.HasIndex(g => g.UpcaseName).HasDatabaseName("ix_groups_upcase_name").IsUnique();
            b.ToTable("groups");
            b.Property(g => g.ConcurrencyStamp).IsConcurrencyToken();
            b.HasMany(o => o.Roles)
                .WithMany(r => r.Groups)
                .UsingEntity(j => j.ToTable("groups_roles"));

            b.HasMany(o => o.Claims)
                .WithOne(o => o.Group);

            b.HasMany(o => o.Users)
                .WithMany(u => u.Groups)
                .UsingEntity(j => j.ToTable("groups_users"));
        });

        builder.Entity<GroupClaim>(b =>
        {
            b.HasKey(o => o.Id);
            b.Property(o => o.ClaimType).HasMaxLength(256);
            b.Property(o => o.ClaimValue);
            b.ToTable("group_claims");
            b.HasOne(o => o.Group).WithMany(g => g.Claims).HasForeignKey(o => o.GroupId).IsRequired();
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
            b.HasMany<UserApiKeyRole>(k => k.UserApiKeyRoles).WithOne(kr => kr.UserApiKey).IsRequired();
        });
    }
}