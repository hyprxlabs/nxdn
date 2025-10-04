using Hyprship.Database.Models;

using Hyprx.AspNetCore.Identity;

using Microsoft.AspNetCore.Identity;

namespace Hyprship.Data.Models;

public class User
{
    public User()
    {
    }

    public User(string email)
    {
        this.UserName = email;
        this.UpcaseUserName = email.ToUpperInvariant();
        this.Email = email;
        this.UpcaseEmail = email.ToUpperInvariant();
    }

    public User(string email, string username)
    {
        this.UserName = username;
        this.UpcaseUserName = username.ToUpperInvariant();
        this.Email = email;
        this.UpcaseEmail = email.ToUpperInvariant();
    }

    /// <summary>
    /// Gets or sets the primary key for this user.
    /// </summary>
    [PersonalData]
    public virtual Guid Id { get; set; } = Uuid7.New();

    /// <summary>
    /// Gets or sets the uppercase username for this user.
    /// </summary>
    public virtual string? UpcaseUserName { get; set; }

    /// <summary>
    /// Gets or sets the normalized username for this user.
    /// </summary>
    [ProtectedPersonalData]
    public virtual string? UserName { get; set; }

    /// <summary>
    /// Gets or sets the email address for this user.
    /// </summary>
    public virtual string? UpcaseEmail { get; set; }

    /// <summary>
    /// Gets or sets the normalized email address for this user.
    /// </summary>
    [ProtectedPersonalData]
    public virtual string? Email { get; set; }

    /// <summary>
    /// Gets or sets a random value that must change whenever a users credentials change (password changed, login removed).
    /// </summary>
    public virtual string? SecurityStamp { get; set; } = Uuid7.Stamp();

    /// <summary>
    /// Gets or sets a random value that must change whenever a user is persisted to the store.
    /// </summary>
    public virtual string? ConcurrencyStamp { get; set; } = Uuid7.Stamp();

    /// <summary>
    /// Gets or sets a value indicating whether or not this user is a service account.
    /// </summary>
    public bool IsServiceAccount { get; set; }

    /// <summary>
    /// Gets or sets the created at date.
    /// </summary>
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the uppdated at date.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the last login at date.
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    public UserPasswordAuth? PasswordAuth { get; set; }

    public HashSet<UserClaim> Claims { get; set; } = new();

    public HashSet<UserLoginProvider> LoginProviders { get; set; } = new();

    public HashSet<UserLoginProviderToken> LoginProviderTokens { get; set; } = new();

    public HashSet<UserPasskey> Passkeys { get; set; } = new();

    public HashSet<Role> Roles { get; set; } = new();

    public HashSet<UserApiKey> ApiKeys { get; set; } = new();

    public HashSet<Group> Groups { get; set; } = new();

    public override string ToString()
        => this.UserName ?? string.Empty;
}