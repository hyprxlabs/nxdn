using Hyprship.Data.Models;

using Microsoft.EntityFrameworkCore;

namespace Hyprship.Data.Stores;

public class GroupStore
{
    public GroupStore(Db db)
    {
        this.Db = db;
    }

    protected Db Db { get; init; }


    public async Task<List<string>> GroupsForUserAsync(User user, CancellationToken token = default)
    {
        if (user.Groups.Count > 0)
            return user.Groups.Select(g => g.Name).Distinct().ToList();

        var groups = from g in this.Db.Groups
                     from u in g.Users
                     where u.Id == user.Id
                     select g.Name;

        return await groups.Distinct().ToListAsync(token);
    }

    public async Task<List<GroupClaim>> ClaimsForUserAsync(User user, CancellationToken token = default)
    {
        if (user.Groups.Count > 0)
        {
            var res = new List<GroupClaim>();
            foreach (var g in user.Groups)
                res.AddRange(g.Claims);

            if (res.Count > 0)
                return res.ToList();
        }

        var claims = from g in this.Db.Groups
                     from u in g.Users
                     where u.Id == user.Id
                     from c in g.Claims
                     select c;

        return await claims.Distinct().ToListAsync(token);
    }
}