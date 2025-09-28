using Hyprship.Data.Models;

using Hyprx.AspNetCore.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hyprship.Data.Stores;

public class UserStore : Hyprx.AspNetCore.Identity.EntityFrameworkCore.UserStore<User, Role, Db, Guid>
{
    public UserStore(Db context, IdentityErrorDescriber? describer = null)
        : base(context, describer)
    {
    }

    public override Guid ConvertIdFromString(string? id)
    {
        if (id == null)
        {
            return Guid.Empty;
        }

        if (Guid.TryParse(id, out var guid))
            return guid;

        return Guid.Empty;
    }

    public override string? ConvertIdToString(Guid id)
    {
        if (id == Guid.Empty)
            return null;

        return id.ToString();
    }

    protected override Task<IdentityUserPasswordAuth<Guid>> GetUserPasswordAuthAsync(User user)
    {
        if (user.PasswordAuth is not null)
            return Task.FromResult(user.PasswordAuth);

        return base.GetUserPasswordAuthAsync(user);
    }
}