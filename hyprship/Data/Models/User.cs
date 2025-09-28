using Hyprx.AspNetCore.Identity;

namespace Hyprship.Data.Models;

public class User : IdentityUser<Guid>
{
    public virtual IdentityUserPasswordAuth<Guid>? PasswordAuth { get; set; }

    public virtual HashSet<UserApiKey> ApiKeys { get; set; } = new();

    public virtual HashSet<IdentityUserRole<Guid>> UserRoles { get; set; } = new();

    public bool IsServiceAccount { get; set; }
}