using Hyprx.AspNetCore.Identity;

namespace Hyprship.Data.Models;

public class UserPasswordAuth : IdentityUserPasswordAuth<Guid>
{
    public UserPasswordAuth()
    {
        this.ConcurrencyStamp = Guid.CreateVersion7().ToString("N");
        this.SecurityStamp = Guid.CreateVersion7().ToString("N");
        this.CreatedAt = DateTime.UtcNow;
    }

    public UserPasswordAuth(Guid userId)
    {
        this.UserId = userId;
        this.ConcurrencyStamp = Guid.CreateVersion7().ToString("N");
        this.SecurityStamp = Guid.CreateVersion7().ToString("N");
        this.CreatedAt = DateTime.UtcNow;
    }

    public UserPasswordAuth(Guid userId, string passwordDigest)
    {
        this.UserId = userId;
        this.ConcurrencyStamp = Guid.CreateVersion7().ToString("N");
        this.SecurityStamp = Guid.CreateVersion7().ToString("N");
        this.CreatedAt = DateTime.UtcNow;
    }

    public DateTime? ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}