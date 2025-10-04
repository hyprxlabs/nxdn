using Hyprship.Database.Models;

namespace Hyprship.Data.Models;

public class Group
{
    public Group()
    {
    }

    public Group(string name)
    {
        this.Name = name;
        this.UpcaseName = name.ToUpperInvariant();
    }

    public virtual Guid Id { get; set; } = Uuid7.New();

    public virtual string Name { get; set; } = string.Empty;

    public virtual string UpcaseName { get; set; } = string.Empty;

    public virtual string ConcurrencyStamp { get; set; } = Uuid7.Stamp();

    public virtual HashSet<Role> Roles { get; set; } = new();

    public virtual HashSet<User> Users { get; set; } = new();

    public virtual HashSet<User> Admins { get; set; } = new();

    public virtual HashSet<GroupClaim> Claims { get; set; } = new();

    public override string ToString()
    {
        return this.Name;
    }
}