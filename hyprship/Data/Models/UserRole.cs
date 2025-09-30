using Hyprx.AspNetCore.Identity;

namespace Hyprship.Data.Models;

public class UserRole : IdentityUserRole<Guid>
{
    public virtual User? User { get; set; } = null!;

    public virtual Role? Role { get; set; } = null!;
}