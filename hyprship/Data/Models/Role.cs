using Hyprship.Database.Models;

using Hyprx.AspNetCore.Identity;

namespace Hyprship.Data.Models;

public class Role : IdentityRole<Guid>
{
    public Role()
    {
        this.Id = Uuid7.New();
        this.ConcurrencyStamp = Uuid7.Stamp();
    }

    public Role(string name)
    {
        this.Id = Uuid7.New();
        this.ConcurrencyStamp = Uuid7.Stamp();
        this.Name = name;
        this.UpcaseName = name.ToUpperInvariant();
    }

    public HashSet<User> Users { get; set; } = new();

    public HashSet<Group> Groups { get; set; } = new();

    public HashSet<RoleClaim> Claims { get; set; } = new();

    public HashSet<UserRole> UserRoles { get; set; } = new();
}