namespace Hyprship.Data.Models;

public class GroupClaim
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public Guid GroupId { get; set; }

    public virtual Group Group { get; set; } = null!;

    public string ClaimType { get; set; } = null!;

    public string ClaimValue { get; set; } = null!;
}