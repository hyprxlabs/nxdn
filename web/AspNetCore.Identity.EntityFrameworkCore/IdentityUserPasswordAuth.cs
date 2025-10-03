// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

using Microsoft.AspNetCore.Identity;

namespace Hyprx.AspNetCore.Identity;

/// <summary>
/// The default implementation of <see cref="IdentityUserPasswordAuth{TKey}"/> which uses a string as a primary key.
/// </summary>
public class IdentityUserPasswordAuth : IdentityUserPasswordAuth<string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityUserPasswordAuth"/> class.
    /// </summary>
    /// <remarks>
    /// The Id property is initialized to form a new GUID string value.
    /// </remarks>
    public IdentityUserPasswordAuth()
    {
        this.UserId = Guid.Empty.ToString();
        this.SecurityStamp = Guid.CreateVersion7().ToString();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityUserPasswordAuth"/> class.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <remarks>
    /// The Id property is initialized to form a new GUID string value.
    /// </remarks>
    public IdentityUserPasswordAuth(string userId)
        : this()
    {
        this.UserId = userId;
    }
}

/// <summary>
/// Represents a user's password authentication information in the identity system.
/// </summary>
/// <typeparam name="TKey">The type used for the primary key for the user.</typeparam>
public class IdentityUserPasswordAuth<TKey>
      where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityUserPasswordAuth{TKey}"/> class.
    /// </summary>
    public IdentityUserPasswordAuth()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityUserPasswordAuth{TKey}"/> class.
    /// </summary>
    /// <param name="id">The user id.</param>
    public IdentityUserPasswordAuth(TKey id)
        : this()
    {
        this.UserId = id;
    }

    /// <summary>
    /// Gets or sets the primary key for this user.
    /// </summary>
    [PersonalData]
    public virtual TKey UserId { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets a whether or not the user's email is confirmed.
    /// </summary>
    /// <value>True if the email address has been confirmed, otherwise false.</value>
    [PersonalData]
    public virtual bool IsEmailVerified { get; set; }

    /// <summary>
    /// Gets or sets a salted and hashed representation of the password for this user.
    /// </summary>
    public virtual string? PasswordDigest { get; set; }

    /// <summary>
    /// Gets or sets a random value that must change whenever a users credentials change (password changed, login removed).
    /// </summary>
    public virtual string? SecurityStamp { get; set; }

    /// <summary>
    /// Gets or sets a random value that must change whenever a user is persisted to the store.
    /// </summary>
    public virtual string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets a telephone number for the user.
    /// </summary>
    [ProtectedPersonalData]
    public virtual string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets a flag indicating if a user has confirmed their telephone address.
    /// </summary>
    /// <value>True if the telephone number has been confirmed, otherwise false.</value>
    [PersonalData]
    public virtual bool IsPhoneNumberVerified { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets a flag indicating if two factor authentication is enabled for this user.
    /// </summary>
    /// <value>True if 2fa is enabled, otherwise false.</value>
    [PersonalData]
    public virtual bool IsTwoFactorEnabled { get; set; }

    /// <summary>
    /// Gets or sets the date and time, in UTC, when any user lockout ends.
    /// </summary>
    /// <remarks>
    /// A value in the past means the user is not locked out.
    /// </remarks>
    public virtual DateTimeOffset? LockoutEndsAt { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets a flag indicating if the user could be locked out.
    /// </summary>
    /// <value>True if the user could be locked out, otherwise false.</value>
    public virtual bool IsLockoutEnabled { get; set; }

    /// <summary>
    /// Gets or sets the number of failed login attempts for the current user.
    /// </summary>
    public virtual int AccessFailedCount { get; set; }

    /// <summary>
    /// Returns the username for this user.
    /// </summary>
    /// <returns>The user id for this user.</returns>
    public override string ToString()
        => this.UserId.ToString() ?? string.Empty;
}