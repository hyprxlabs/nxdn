// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Hyprx.AspNetCore.Identity;

/// <summary>
/// The default implementation of <see cref="IdentityRole{TKey}"/> which uses a string as the primary key.
/// </summary>
public class IdentityRole : IdentityRole<Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityRole"/> class.
    /// </summary>
    /// <remarks>
    /// The Id property is initialized to form a new GUID string value.
    /// </remarks>
    public IdentityRole()
    {
        this.Id = Guid.CreateVersion7();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityRole"/> class.
    /// </summary>
    /// <param name="roleName">The role name.</param>
    /// <remarks>
    /// The Id property is initialized to form a new GUID string value.
    /// </remarks>
    public IdentityRole(string roleName)
        : this()
    {
        this.Name = roleName;
    }
}

/// <summary>
/// Represents a role in the identity system.
/// </summary>
/// <typeparam name="TKey">The type used for the primary key for the role.</typeparam>
public class IdentityRole<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityRole{TKey}"/> class.
    /// </summary>
    public IdentityRole()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityRole{TKey}"/> class.
    /// </summary>
    /// <param name="roleName">The role name.</param>
    public IdentityRole(string roleName)
        : this()
    {
        this.Name = roleName;
    }

    /// <summary>
    /// Gets or sets the primary key for this role.
    /// </summary>
    public virtual TKey Id { get; set; } = default!;

    /// <summary>
    /// Gets or sets the name for this role.
    /// </summary>
    public virtual string? Name { get; set; }

    /// <summary>
    /// Gets or sets the normalized name for this role.
    /// </summary>
    public virtual string? NormalizedName { get; set; }

    /// <summary>
    /// Gets or sets a random value that should change whenever a role is persisted to the store
    /// </summary>
    public virtual string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Returns the name of the role.
    /// </summary>
    /// <returns>The name of the role.</returns>
    public override string ToString()
    {
        return this.Name ?? string.Empty;
    }
}