using System.Security.Claims;

namespace Hyprship.Data.Models;

public class GroupClaim
{
    public GroupClaim()
    {
    }

    public GroupClaim(Guid groupId)
    {
        this.GroupId = groupId;
    }

    public virtual int Id { get; set; }

    public virtual Guid GroupId { get; set; }

    public virtual string? ClaimType { get; set; } = null!;

    public virtual string? ClaimValue { get; set; } = null!;

    public virtual Group Group { get; set; } = null!;

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