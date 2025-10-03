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

    public virtual DateTime? ExpiresAt { get; set; }

    public virtual DateTime CreatedAt { get; set; }

    public virtual DateTime? UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}