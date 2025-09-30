using Hyprx.AspNetCore.Identity;

namespace Hyprship.Data.Models;

public class UserToken : IdentityUserToken<Guid>
{
    public virtual User? User { get; set; } = null;
}