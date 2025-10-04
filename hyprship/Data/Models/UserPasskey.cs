using Hyprship.Database.Models;

using Hyprx.AspNetCore.Identity;

namespace Hyprship.Data.Models;

public class UserPasskey
{
    public UserPasskey()
    {
    }

    public UserPasskey(Guid userId)
    {
        this.UserId = userId;
    }

    public UserPasskey(Guid userId, byte[] credentialId)
    {
        this.UserId = userId;
        this.CredentialId = credentialId;
    }

    /// <summary>
    /// Gets or sets the primary key of the user that owns this passkey.
    /// </summary>
    public virtual Guid UserId { get; set; } = Uuid7.New();

    /// <summary>
    /// Gets or sets the credential ID for this passkey.
    /// </summary>
    public virtual byte[] CredentialId { get; set; } = [];

    /// <summary>
    /// Gets or sets additional data associated with this passkey.
    /// </summary>
    public virtual IdentityPasskeyData Data { get; set; } = default!;

    /// <summary>
    /// Gets or sets the created at date.
    /// </summary>
    public virtual DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the updated at date.
    /// </summary>
    public virtual DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the user.
    /// </summary>
    public virtual User? User { get; set; } = null;
}