using System.Security.Claims;

using Hyprship.Data.Models;
using Hyprship.Data.Stores;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Hypership.Services.Identity;

public class HyprshipClaimsFactory : Microsoft.AspNetCore.Identity.UserClaimsPrincipalFactory<User, Role>
{
    public HyprshipClaimsFactory(UserManager<User> userManager, RoleManager<Role> roleManager, GroupStore groupStore, IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
    {
    }

    protected override Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {
        return base.GenerateClaimsAsync(user);
    }
}