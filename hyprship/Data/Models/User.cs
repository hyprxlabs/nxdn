using Hyprx.AspNetCore.Identity;

namespace Hyprship.Data.Models;

public class User : IdentityUser<Guid>
{
    public User()
    {
        this.Id = Guid.CreateVersion7();
        this.ConcurrencyStamp = Guid.CreateVersion7().ToString("N");
        this.SecurityStamp = Guid.NewGuid().ToString("N");
    }

    public bool IsServiceAccount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public virtual IdentityUserPasswordAuth<Guid>? PasswordAuth { get; set; }

    public virtual HashSet<UserApiKey> ApiKeys { get; set; } = new();

    public virtual HashSet<IdentityUserRole<Guid>> UserRoles { get; set; } = new();


    public virtual HashSet<Group> Groups { get; set; } = new();


    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}