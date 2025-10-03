using Hyprx.AspNetCore.Identity;

namespace Hyprship.Data.Models;

public class UserPasskey : IdentityUserPasskey<Guid>
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

    public virtual User User { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}