namespace Hyprx.AspNetCore.Identity;

public class RoleClaim : IdentityRoleClaim<Guid>
{
    public RoleClaim()
    {
    }

    public RoleClaim(Guid roleId)
    {
        this.RoleId = roleId;
    }

    public RoleClaim(Guid roleId, string claimType, string claimValue)
    {
        this.RoleId = roleId;
        this.ClaimType = claimType;
        this.ClaimValue = claimValue;
    }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}