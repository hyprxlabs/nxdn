using Hyprx.AspNetCore.Identity;

namespace Hyprship.Data.Models;

public class UserPasskey : IdentityUserPasskey<Guid>
{
    public virtual User User { get; set; } = null!;
}