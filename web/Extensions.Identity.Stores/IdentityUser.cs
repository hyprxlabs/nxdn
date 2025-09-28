// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

using Microsoft.AspNetCore.Identity;

namespace Hyprx.AspNetCore.Identity;

/// <summary>
/// The default implementation of <see cref="IdentityUser{TKey}"/> which uses a string as a primary key.
/// </summary>
public class IdentityUser : IdentityUser<Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityUser"/> class.
    /// </summary>
    /// <remarks>
    /// The Id property is initialized to form a new GUID string value.
    /// </remarks>
    public IdentityUser()
    {
        this.Id = Guid.CreateVersion7();
        this.SecurityStamp = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityUser"/> class.
    /// </summary>
    /// <param name="userName">The user name.</param>
    /// <remarks>
    /// The Id property is initialized to form a new GUID string value.
    /// </remarks>
    public IdentityUser(string userName)
        : this()
    {
        this.UserName = userName;
    }
}

/// <summary>
/// Represents a user in the identity system.
/// </summary>
/// <typeparam name="TKey">The type used for the primary key for the user.</typeparam>
public class IdentityUser<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityUser{TKey}"/> class.
    /// </summary>
    public IdentityUser()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityUser{TKey}"/> class.
    /// </summary>
    /// <param name="userName">The user name.</param>
    public IdentityUser(string userName)
        : this()
    {
        this.UserName = userName;
    }

    /// <summary>
    /// Gets or sets the primary key for this user.
    /// </summary>
    [PersonalData]
    public virtual TKey Id { get; set; } = default!;

    /// <summary>
    /// Gets or sets the user name for this user.
    /// </summary>
    [ProtectedPersonalData]
    public virtual string? FormattedUserName { get; set; }

    /// <summary>
    /// Gets or sets the normalized user name for this user.
    /// </summary>
    public virtual string? UserName { get; set; }

    /// <summary>
    /// Gets or sets the email address for this user.
    /// </summary>
    [ProtectedPersonalData]
    public virtual string? FormattedEmail { get; set; }

    /// <summary>
    /// Gets or sets the normalized email address for this user.
    /// </summary>
    public virtual string? Email { get; set; }

    /// <summary>
    /// Gets or sets a random value that must change whenever a users credentials change (password changed, login removed).
    /// </summary>
    public virtual string? SecurityStamp { get; set; }

    /// <summary>
    /// Gets or sets a random value that must change whenever a user is persisted to the store.
    /// </summary>
    public virtual string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

    public override string ToString()
        => this.UserName ?? string.Empty;
}