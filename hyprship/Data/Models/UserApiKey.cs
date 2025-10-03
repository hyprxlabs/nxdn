using System.ComponentModel.DataAnnotations.Schema;

using Hyprship.Database.Models;

using Hyprx.AspNetCore.Identity;

namespace Hyprship.Data.Models;

public class UserApiKey : IdentityUserApiKey<Guid>
{
    private HashSet<UserApiKeyRole> userApiKeyRoles = new();

    public UserApiKey()
    {
        this.Id = Guid.CreateVersion7();
        this.UserApiKeyRoles = new();
        this.CreatedAt = DateTime.UtcNow;
    }

    public UserApiKey(Guid userId)
        : this()
    {
        this.Id = Guid.CreateVersion7();
        this.UserApiKeyRoles = new();
        this.UserId = userId;
        this.CreatedAt = DateTime.UtcNow;
    }

    public virtual User? User { get; set; } = null;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual HashSet<UserApiKeyRole> UserApiKeyRoles
    {
        get => this.userApiKeyRoles;
        set 
            {
                this.userApiKeyRoles = value;
                this.Roles = new Many<UserApiKey, Role, UserApiKeyRole>(
                    this,
                    this.UserApiKeyRoles,
                    (o) => o.Role,
                    (key, role) => new(key.Id, role.Id)
                    {
                        Role = role,
                        UserApiKey = this,
                    });
        }
    }

    [NotMapped]
    public Many<UserApiKey, Role, UserApiKeyRole> Roles { get; private set; }
}

