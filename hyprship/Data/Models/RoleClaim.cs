using System.Security.Claims;

using Hyprship.Database.Models;

namespace Hyprship.Data.Models;

public class RoleClaim
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

    /// <summary>
    /// Gets or sets the identifier for this role claim.
    /// </summary>
    public virtual int Id { get; set; } = 0;

    /// <summary>
    /// Gets or sets the of the primary key of the role associated with this claim.
    /// </summary>
    public virtual Guid RoleId { get; set; } = Uuid7.New();

    /// <summary>
    /// Gets or sets the claim type for this claim.
    /// </summary>
    public virtual string? ClaimType { get; set; }

    /// <summary>
    /// Gets or sets the claim value for this claim.
    /// </summary>
    public virtual string? ClaimValue { get; set; }

    /// <summary>
    /// Gets or sets the created at date.
    /// </summary>
    public virtual DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the updated at date.
    /// </summary>
    public virtual DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the role associated with this claim.
    /// </summary>
    public virtual Role Role { get; set; } = default!;

    /// <summary>
    /// Constructs a new claim with the type and value.
    /// </summary>
    /// <returns>The <see cref="Claim"/> that was produced.</returns>
    public virtual Claim ToClaim()
    {
        return new Claim(this.ClaimType!, this.ClaimValue!);
    }

    /// <summary>
    /// Initializes by copying ClaimType and ClaimValue from the other claim.
    /// </summary>
    /// <param name="other">The claim to initialize from.</param>
    public virtual void InitializeFromClaim(Claim? other)
    {
        this.ClaimType = other?.Type;
        this.ClaimValue = other?.Value;
    }
}