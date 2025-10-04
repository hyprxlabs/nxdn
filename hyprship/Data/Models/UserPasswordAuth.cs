using Hyprship.Database.Models;

using Hyprx.AspNetCore.Identity;

using Microsoft.AspNetCore.Identity;

namespace Hyprship.Data.Models;

public class UserPasswordAuth
{
    public UserPasswordAuth()
    {
    }

    public UserPasswordAuth(Guid userId)
    {
        this.UserId = userId;
    }

    public UserPasswordAuth(Guid userId, string passwordDigest)
    {
        this.UserId = userId;
        this.PasswordDigest = passwordDigest;
    }

    /// <summary>
    /// Gets or sets the primary key for this user.
    /// </summary>
    [PersonalData]
    public virtual Guid UserId { get; set; } = Uuid7.New();

    /// <summary>
    /// Gets or sets a value indicating whether the user's email is confirmed.
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
    public virtual string? ConcurrencyStamp { get; set; } = Uuid7.Stamp();

    /// <summary>
    /// Gets or sets a telephone number for the user.
    /// </summary>
    [ProtectedPersonalData]
    public virtual string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a flag indicating if a user has confirmed their telephone address.
    /// </summary>
    /// <value>True if the telephone number has been confirmed, otherwise false.</value>
    [PersonalData]
    public virtual bool IsPhoneNumberVerified { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a flag indicating if two-factor
    /// authentication is enabled for this user.
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
    /// Gets or sets a value indicating whether a flag indicating if the user could be locked out.
    /// </summary>
    /// <value>True if the user could be locked out, otherwise false.</value>
    public virtual bool IsLockoutEnabled { get; set; }

    /// <summary>
    /// Gets or sets the number of failed login attempts for the current user.
    /// </summary>
    public virtual int AccessFailedCount { get; set; }

    public virtual DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual DateTime? UpdatedAt { get; set; }

    public virtual DateTime? ExpiresAt { get; set; }

    public virtual User User { get; set; } = null!;

    /// <summary>
    /// Returns the username for this user.
    /// </summary>
    /// <returns>The user id for this user.</returns>
    public override string ToString()
        => this.UserId.ToString() ?? string.Empty;
}