using System.Security.Claims;

using Hyprx.AspNetCore.Identity;

namespace Hyprship.Data.Models;

public class UserClaim
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

    /// <summary>
    /// Gets or sets the identifier for this user claim.
    /// </summary>
    public virtual int Id { get; set; } = default!;

    /// <summary>
    /// Gets or sets the primary key of the user associated with this claim.
    /// </summary>
    public virtual Guid UserId { get; set; } = Guid.Empty;

    /// <summary>
    /// Gets or sets the claim type for this claim.
    /// </summary>
    public virtual string? ClaimType { get; set; }

    /// <summary>
    /// Gets or sets the claim value for this claim.
    /// </summary>
    public virtual string? ClaimValue { get; set; }

    /// <summary>
    /// Converts the entity into a Claim instance.
    /// </summary>
    /// <returns>The claim.</returns>
    public virtual Claim ToClaim()
    {
        return new Claim(this.ClaimType!, this.ClaimValue!);
    }

    /// <summary>
    /// Reads the type and value from the Claim.
    /// </summary>
    /// <param name="claim">The Claim instance.</param>
    public virtual void InitializeFromClaim(Claim claim)
    {
        this.ClaimType = claim.Type;
        this.ClaimValue = claim.Value;
    }

    /// <summary>
    /// Gets or sets the created at value.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the updated at value.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the user.
    /// </summary>
    public virtual User? User { get; set; }

    public override string ToString()
    {
        return $"{this.ClaimType}  {this.ClaimValue}";
    }
}