using Hyprship.Database.Models;

namespace Hyprship.Data.Models;

public class Group
{
    public Group()
    {
        this.Id = Uuid7.New();
        this.ConcurrencyStamp = Uuid7.Stamp();
    }

    public Group(string name)
    {
        this.Id = Uuid7.New();
        this.ConcurrencyStamp = Uuid7.Stamp();
        this.Name = name;
        this.UpcaseName = name.ToUpperInvariant();
    }

    public virtual Guid Id { get; set; }

    public virtual string Name { get; set; } = null!;

    public virtual string UpcaseName { get; set; } = null!;

    public virtual string ConcurrencyStamp { get; set; }

    public virtual HashSet<Role> Roles { get; set; } = new();

    public virtual HashSet<User> Users { get; set; } = new();

    public virtual HashSet<User> Admins { get; set; } = new();

    public virtual HashSet<GroupClaim> Claims { get; set; } = new();
}