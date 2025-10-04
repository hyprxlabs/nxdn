using Hyprx.AspNetCore.Identity;

namespace Hyprship.Data.Models;

public class UserLoginProvider
{
    public UserLoginProvider()
    {
    }

    public UserLoginProvider(Guid userId)
    {
        this.UserId = userId;
    }

    public UserLoginProvider(Guid userId, string loginProvider, string providerKey)
    {
        this.UserId = userId;
        this.LoginProvider = loginProvider;
        this.ProviderKey = providerKey;
    }

    /// <summary>
    /// Gets or sets the primary key of the user associated with this login.
    /// </summary>
    public virtual Guid UserId { get; set; } = Guid.Empty;

    /// <summary>
    /// Gets or sets the login provider for the login (e.g. facebook, google).
    /// </summary>
    public virtual string LoginProvider { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique provider identifier for this login.
    /// </summary>
    public virtual string ProviderKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the friendly name used in a UI for this login.
    /// </summary>
    public virtual string? ProviderDisplayName { get; set; }

    /// <summary>
    /// Gets or sets the created at date.
    /// </summary>
    public virtual DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets or the updated at date.
    /// </summary>
    public virtual DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the user.
    /// </summary>
    public virtual User User { get; set; } = null!;
}