using Hyprship.Data.Models;

using Hyprx.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Hypership.Services.Identity;

public class RoleManager : RoleManager<Role>
{
    public RoleManager(
        IRoleStore<Role> store,
        IEnumerable<IRoleValidator<Role>> roleValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        ILogger<RoleManager<Role>> logger)
        : base(store, roleValidators, keyNormalizer, errors, logger)
    {
    }
}