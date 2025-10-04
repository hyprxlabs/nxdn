using System.ComponentModel.DataAnnotations.Schema;

using Hyprship.Database.Models;

using Hyprx.AspNetCore.Identity;

namespace Hyprship.Data.Models;

public class UserApiKey
{
    public UserApiKey()
    {
    }

    public UserApiKey(Guid userId)
    {
        this.UserId = userId;
    }

    /// <summary>
    /// Gets or sets the id of the api key.
    /// </summary>
    public virtual Guid Id { get; set; } = Uuid7.New();

    /// <summary>
    /// Gets or sets the user id of the api key.
    /// </summary>
    public virtual Guid UserId { get; set; } = Guid.Empty;

    /// <summary>
    /// Gets or sets the name of the api key.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Gets or sets the api key's hashed value.
    /// </summary>
    public virtual string KeyDigest { get; set; } = default!;

    /// <summary>
    /// Gets or sets the expires at date.
    /// </summary>
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Gets or sets the created at date.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the updated at date.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the user that owns the api key.
    /// </summary>
    public virtual User? User { get; set; } = null;

    /// <summary>
    /// Gets or sets the roles assigned to the api key.
    /// </summary>
    public virtual HashSet<Role> Roles { get; set; } = new();
}