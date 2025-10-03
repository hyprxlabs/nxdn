using Hyprx.AspNetCore.Identity;

namespace Hyprship.Data.Models;

public class UserRole : IdentityUserRole<Guid>
{
    public UserRole()
    {
    }

    public UserRole(Guid userId, Guid roleId)
    {
        this.UserId = userId;
        this.RoleId = roleId;
    }

    public UserRole(User user, Role role)
    {
        this.User = user;
        this.Role = role;
        this.UserId = user.Id;
        this.RoleId = role.Id;
    }

    public virtual User? User { get; set; } = null!;

    public virtual Role? Role { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}