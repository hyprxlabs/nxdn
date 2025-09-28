// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;

using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Hyprx.AspNetCore.Identity.EntityFrameworkCore;

/// <summary>
/// Base class for the Entity Framework database context used for identity.
/// </summary>
/// <typeparam name="TUser">The type of the user objects.</typeparam>
public class IdentityUserContext<TUser> : IdentityUserContext<TUser, Guid>
    where TUser : IdentityUser
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityUserContext{TUser}"/> class.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    public IdentityUserContext(DbContextOptions options)
        : base(options)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityUserContext{TUser}" /> class.
    /// </summary>
    protected IdentityUserContext()
    {
    }
}

/// <summary>
/// Base class for the Entity Framework database context used for identity.
/// </summary>
/// <typeparam name="TUser">The type of user objects.</typeparam>
/// <typeparam name="TKey">The type of the primary key for users and roles.</typeparam>
public class IdentityUserContext<TUser, TKey> : IdentityUserContext<TUser, TKey, IdentityUserClaim<TKey>, IdentityUserPasswordAuth<TKey>, IdentityUserLoginProvider<TKey>, IdentityUserToken<TKey>>
    where TUser : IdentityUser<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityUserContext{TUser, TKey}"/> class.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    public IdentityUserContext(DbContextOptions options)
        : base(options)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityUserContext{TUser, TKey}"/> class.
    /// </summary>
    protected IdentityUserContext()
    {
    }
}

/// <summary>
/// Base class for the Entity Framework database context used for identity.
/// </summary>
/// <typeparam name="TUser">The type of user objects.</typeparam>
/// <typeparam name="TKey">The type of the primary key for users and roles.</typeparam>
/// <typeparam name="TUserClaim">The type of the user claim object.</typeparam>
/// <typeparam name="TUserPasswordAuth">The type of the user password authentication object.</typeparam>
/// <typeparam name="TUserLogin">The type of the user login object.</typeparam>
/// <typeparam name="TUserToken">The type of the user token object.</typeparam>
public class IdentityUserContext<TUser, TKey, TUserClaim, TUserPasswordAuth, TUserLogin, TUserToken> : IdentityUserContext<TUser, TKey, TUserClaim, TUserPasswordAuth, TUserLogin, TUserToken, IdentityUserPasskey<TKey>>
    where TUser : IdentityUser<TKey>
    where TKey : IEquatable<TKey>
    where TUserClaim : IdentityUserClaim<TKey>
    where TUserPasswordAuth : IdentityUserPasswordAuth<TKey>
    where TUserLogin : IdentityUserLoginProvider<TKey>
    where TUserToken : IdentityUserToken<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityUserContext{TUser, TKey, TUserClaim, TUserPasswordAuth, TUserLogin, TUserToken}"/> class.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    public IdentityUserContext(DbContextOptions options)
        : base(options)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityUserContext{TUser, TKey, TUserClaim, TUserPasswordAuth, TUserLogin, TUserToken}"/> class.
    /// </summary>
    protected IdentityUserContext()
    {
    }
}

/// <summary>
/// Base class for the Entity Framework database context used for identity.
/// </summary>
/// <typeparam name="TUser">The type of user objects.</typeparam>
/// <typeparam name="TKey">The type of the primary key for users and roles.</typeparam>
/// <typeparam name="TUserClaim">The type of the user claim object.</typeparam>
/// <typeparam name="TUserPasswordAuth">The type of the user password authentication object.</typeparam>
/// <typeparam name="TUserLogin">The type of the user login object.</typeparam>
/// <typeparam name="TUserToken">The type of the user token object.</typeparam>
/// <typeparam name="TUserPasskey">The type of the user passkey object.</typeparam>
public abstract class IdentityUserContext<TUser, TKey, TUserClaim, TUserPasswordAuth, TUserLogin, TUserToken, TUserPasskey> : DbContext
    where TUser : IdentityUser<TKey>
    where TKey : IEquatable<TKey>
    where TUserClaim : IdentityUserClaim<TKey>
    where TUserPasswordAuth : IdentityUserPasswordAuth<TKey>
    where TUserLogin : IdentityUserLoginProvider<TKey>
    where TUserToken : IdentityUserToken<TKey>
    where TUserPasskey : IdentityUserPasskey<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityUserContext{TUser, TKey, TUserClaim, TUserPasswordAuth, TUserLogin, TUserToken, TUserPasskey}"/> class.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    public IdentityUserContext(DbContextOptions options)
        : base(options)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityUserContext{TUser, TKey, TUserClaim, TUserPasswordAuth, TUserLogin, TUserToken, TUserPasskey}"/> class.
    /// </summary>
    protected IdentityUserContext()
    {
    }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> of Users.
    /// </summary>
    public virtual DbSet<TUser> Users { get; set; } = default!;

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> of User claims.
    /// </summary>
    public virtual DbSet<TUserClaim> UserClaims { get; set; } = default!;

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> of User logins.
    /// </summary>
    public virtual DbSet<TUserLogin> UserLogins { get; set; } = default!;

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> of User tokens.
    /// </summary>
    public virtual DbSet<TUserToken> UserTokens { get; set; } = default!;

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> of User passkeys.
    /// </summary>
    public virtual DbSet<TUserPasskey> UserPasskeys { get; set; } = default!;

    /// <summary>
    /// Gets the schema version used for versioning.
    /// </summary>
    protected virtual Version SchemaVersion => this.GetStoreOptions()?.SchemaVersion ?? IdentitySchemaVersions.Version1;

    private StoreOptions? GetStoreOptions() => this.GetService<IDbContextOptions>()
                        .Extensions.OfType<CoreOptionsExtension>()
                        .FirstOrDefault()?.ApplicationServiceProvider
                        ?.GetService<IOptions<IdentityOptions>>()
                        ?.Value?.Stores;

    private sealed class PersonalDataConverter : ValueConverter<string, string>
    {
        public PersonalDataConverter(IPersonalDataProtector protector)
            : base(s => protector.Protect(s), s => protector.Unprotect(s), default)
        {
        }
    }

    /// <summary>
    /// Configures the schema needed for the identity framework for a specific schema version.
    /// </summary>
    /// <param name="builder">
    /// The builder being used to construct the model for this context.
    /// </param>
    /// <param name="schemaVersion">The schema version.</param>
    internal virtual void OnModelCreatingVersion(ModelBuilder builder, Version schemaVersion)
    {
        if (schemaVersion >= IdentitySchemaVersions.Version3)
        {
            this.OnModelCreatingVersion3(builder);
        }
        else if (schemaVersion >= IdentitySchemaVersions.Version2)
        {
            this.OnModelCreatingVersion2(builder);
        }
        else
        {
            this.OnModelCreatingVersion1(builder);
        }
    }

    /// <summary>
    /// Configures the schema needed for the identity framework for schema version 3.0.
    /// </summary>
    /// <param name="builder">
    /// The builder being used to construct the model for this context.
    /// </param>
    internal virtual void OnModelCreatingVersion3(ModelBuilder builder)
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

        builder.Entity<TUser>(b =>
        {
            b.HasKey(u => u.Id);
            b.HasIndex(u => u.UserName).HasDatabaseName("UserNameIndex").IsUnique();
            b.HasIndex(u => u.Email).HasDatabaseName("EmailIndex");
            b.ToTable("IdentityUsers");
            b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

            b.Property(u => u.FormattedUserName).HasMaxLength(256);
            b.Property(u => u.UserName).HasMaxLength(256);
            b.Property(u => u.FormattedEmail).HasMaxLength(256);
            b.Property(u => u.Email).HasMaxLength(256);

            if (encryptPersonalData)
            {
                converter = new PersonalDataConverter(this.GetService<IPersonalDataProtector>());
                var personalDataProps = typeof(TUser).GetProperties().Where(
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

            b.HasMany<TUserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
            b.HasMany<TUserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
            b.HasMany<TUserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
            b.HasOne<TUserPasswordAuth>().WithOne().HasForeignKey<TUserPasswordAuth>(ua => ua.UserId).IsRequired();
            b.HasMany<TUserPasskey>().WithOne().HasForeignKey(up => up.UserId).IsRequired();
        });

        builder.Entity<TUserPasswordAuth>(b =>
        {
            b.HasKey(ua => ua.UserId);
            b.Property(u => u.PhoneNumber).HasMaxLength(256);
            b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

            b.ToTable("IdentityUserPasswordAuth");
            if (encryptPersonalData)
            {
                converter = new PersonalDataConverter(this.GetService<IPersonalDataProtector>());
                var personalDataProps = typeof(TUserPasswordAuth).GetProperties().Where(
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

        builder.Entity<TUserClaim>(b =>
        {
            b.HasKey(uc => uc.Id);
            b.ToTable("IdentityUserClaims");
        });

        builder.Entity<TUserLogin>(b =>
        {
            b.HasKey(l => new { l.LoginProvider, l.ProviderKey });

            if (maxKeyLength > 0)
            {
                b.Property(l => l.LoginProvider).HasMaxLength(maxKeyLength);
                b.Property(l => l.ProviderKey).HasMaxLength(maxKeyLength);
            }

            b.ToTable("IdentityUserLogins");
        });

        builder.Entity<TUserToken>(b =>
        {
            b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            if (maxKeyLength > 0)
            {
                b.Property(t => t.LoginProvider).HasMaxLength(maxKeyLength);
                b.Property(t => t.Name).HasMaxLength(maxKeyLength);
            }

            if (encryptPersonalData)
            {
                var tokenProps = typeof(TUserToken).GetProperties().Where(
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

            b.ToTable("IdentityUserTokens");
        });

        builder.Entity<TUserPasskey>(b =>
        {
            b.HasKey(p => p.CredentialId);
            b.ToTable("IdentityUserPasskeys");
            b.Property(p => p.CredentialId).HasMaxLength(1024); // Defined in WebAuthn spec to be no longer than 1023 bytes
            b.OwnsOne(p => p.Data).ToJson();
        });
    }

    /// <summary>
    /// Configures the schema needed for the identity framework for schema version 2.0.
    /// </summary>
    /// <param name="builder">
    /// The builder being used to construct the model for this context.
    /// </param>
    internal virtual void OnModelCreatingVersion2(ModelBuilder builder)
    {
        // Differences from Version 1:
        // - maxKeyLength defaults to 128
        // - PhoneNumber has a 256 max length
        var storeOptions = this.GetStoreOptions();
        var maxKeyLength = storeOptions?.MaxLengthForKeys ?? 0;
        if (maxKeyLength == 0)
        {
            maxKeyLength = 128;
        }

        var encryptPersonalData = storeOptions?.ProtectPersonalData ?? false;
        PersonalDataConverter? converter = null;

        builder.Entity<TUser>(b =>
        {
            b.HasKey(u => u.Id);
            b.HasIndex(u => u.UserName).HasDatabaseName("UserNameIndex").IsUnique();
            b.HasIndex(u => u.Email).HasDatabaseName("EmailIndex");
            b.ToTable("IdentityUsers");
            b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

            b.Property(u => u.FormattedUserName).HasMaxLength(256);
            b.Property(u => u.UserName).HasMaxLength(256);
            b.Property(u => u.FormattedEmail).HasMaxLength(256);
            b.Property(u => u.Email).HasMaxLength(256);

            if (encryptPersonalData)
            {
                converter = new PersonalDataConverter(this.GetService<IPersonalDataProtector>());
                var personalDataProps = typeof(TUser).GetProperties().Where(
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

            b.HasMany<TUserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
            b.HasMany<TUserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
            b.HasMany<TUserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
            b.HasOne<TUserPasswordAuth>().WithOne().HasForeignKey<TUserPasswordAuth>(ua => ua.UserId).IsRequired();
        });

        builder.Entity<TUserPasswordAuth>(b =>
        {
            b.HasKey(ua => ua.UserId);
            b.Property(u => u.PhoneNumber).HasMaxLength(256);
            b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

            b.ToTable("IdentityUserPasswordAuth");
            if (encryptPersonalData)
            {
                converter = new PersonalDataConverter(this.GetService<IPersonalDataProtector>());
                var personalDataProps = typeof(TUserPasswordAuth).GetProperties().Where(
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

        builder.Entity<TUserClaim>(b =>
        {
            b.HasKey(uc => uc.Id);
            b.ToTable("IdentityUserClaims");
        });

        builder.Entity<TUserLogin>(b =>
        {
            b.HasKey(l => new { l.LoginProvider, l.ProviderKey });

            if (maxKeyLength > 0)
            {
                b.Property(l => l.LoginProvider).HasMaxLength(maxKeyLength);
                b.Property(l => l.ProviderKey).HasMaxLength(maxKeyLength);
            }

            b.ToTable("IdentityUserLogins");
        });

        builder.Entity<TUserToken>(b =>
        {
            b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            if (maxKeyLength > 0)
            {
                b.Property(t => t.LoginProvider).HasMaxLength(maxKeyLength);
                b.Property(t => t.Name).HasMaxLength(maxKeyLength);
            }

            if (encryptPersonalData)
            {
                var tokenProps = typeof(TUserToken).GetProperties().Where(
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

            b.ToTable("IdentityUserTokens");
        });

        builder.Ignore<TUserPasskey>();
    }

    /// <summary>
    /// Configures the schema needed for the identity framework for schema version 1.0.
    /// </summary>
    /// <param name="builder">
    /// The builder being used to construct the model for this context.
    /// </param>
    internal virtual void OnModelCreatingVersion1(ModelBuilder builder)
    {
        var storeOptions = this.GetStoreOptions();
        var maxKeyLength = storeOptions?.MaxLengthForKeys ?? 0;
        var encryptPersonalData = storeOptions?.ProtectPersonalData ?? false;
        PersonalDataConverter? converter = null;

        builder.Entity<TUser>(b =>
        {
            b.HasKey(u => u.Id);
            b.HasIndex(u => u.UserName).HasDatabaseName("UserNameIndex").IsUnique();
            b.HasIndex(u => u.Email).HasDatabaseName("EmailIndex");
            b.ToTable("IdentityUsers");
            b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

            b.Property(u => u.FormattedUserName).HasMaxLength(256);
            b.Property(u => u.UserName).HasMaxLength(256);
            b.Property(u => u.FormattedEmail).HasMaxLength(256);
            b.Property(u => u.Email).HasMaxLength(256);

            if (encryptPersonalData)
            {
                converter = new PersonalDataConverter(this.GetService<IPersonalDataProtector>());
                var personalDataProps = typeof(TUser).GetProperties().Where(
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

            b.HasMany<TUserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
            b.HasMany<TUserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
            b.HasMany<TUserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
        });

        builder.Entity<TUserClaim>(b =>
        {
            b.HasKey(uc => uc.Id);
            b.ToTable("IdentityUserClaims");
        });

        builder.Entity<TUserLogin>(b =>
        {
            b.HasKey(l => new { l.LoginProvider, l.ProviderKey });

            if (maxKeyLength > 0)
            {
                b.Property(l => l.LoginProvider).HasMaxLength(maxKeyLength);
                b.Property(l => l.ProviderKey).HasMaxLength(maxKeyLength);
            }

            b.ToTable("IdentityUserLogins");
        });

        builder.Entity<TUserToken>(b =>
        {
            b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            if (maxKeyLength > 0)
            {
                b.Property(t => t.LoginProvider).HasMaxLength(maxKeyLength);
                b.Property(t => t.Name).HasMaxLength(maxKeyLength);
            }

            if (encryptPersonalData)
            {
                var tokenProps = typeof(TUserToken).GetProperties().Where(
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

            b.ToTable("IdentityUserTokens");
        });

        builder.Ignore<TUserPasskey>();
    }

    /// <summary>
    /// Configures the schema needed for the identity framework.
    /// </summary>
    /// <param name="builder">
    /// The builder being used to construct the model for this context.
    /// </param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        var version = this.GetStoreOptions()?.SchemaVersion ?? IdentitySchemaVersions.Version1;
        this.OnModelCreatingVersion(builder, version);
    }
}