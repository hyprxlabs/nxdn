using Hyprship.Database.Models;

using Hyprx.AspNetCore.Identity;

namespace Hyprship.Data.Models;

public class Role : IdentityRole<Guid>
{
    public Role()
    {
    }

    public Role(string name)
    {
        this.Name = name;
        this.UpcaseName = name.ToUpperInvariant();
    }

    /// <summary>
    /// Gets or sets the primary key for this role.
    /// </summary>
    public virtual Guid Id { get; set; } = Uuid7.New();

    /// <summary>
    /// Gets or sets the name for this role.
    /// </summary>
    public virtual string? UpcaseName { get; set; }

    /// <summary>
    /// Gets or sets the normalized name for this role.
    /// </summary>
    public virtual string? Name { get; set; }

    /// <summary>
    /// Gets or sets a random value that should change whenever a role is persisted to the store.
    /// </summary>
    public virtual string? ConcurrencyStamp { get; set; } = Uuid7.Stamp();

    public HashSet<User> Users { get; set; } = new();

    public HashSet<RoleClaim> Claims { get; set; } = new();

    public HashSet<UserApiKey> UserApiKeys { get; set; } = new();

    public HashSet<Group> Groups { get; set; } = new();

    /// <summary>
    /// Returns the name of the role.
    /// </summary>
    /// <returns>The name of the role.</returns>
    public override string ToString()
    {
        return this.Name ?? string.Empty;
    }
}