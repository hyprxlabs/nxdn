using Hyprx.AspNetCore.Identity;

namespace Hyprship.Data.Models;

public class UserClaim : IdentityUserClaim<Guid>
{
    public UserClaim()
    {
    }

    public UserClaim(Guid userId)
    {
        this.UserId = userId;
    }

    public UserClaim(Guid userId, string claimType, string claimValue)
    {
        this.UserId = userId;
        this.ClaimType = claimType;
        this.ClaimValue = claimValue;
    }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}