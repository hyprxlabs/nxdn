using Hyprship.Data.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hyprship.Data.Stores;

public class RoleStore : Hyprx.AspNetCore.Identity.EntityFrameworkCore.RoleStore<Role, Db, Guid>
{
    public RoleStore(Db context, IdentityErrorDescriber? describer = null)
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
}