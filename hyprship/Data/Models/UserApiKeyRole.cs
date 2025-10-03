namespace Hyprship.Data.Models;

public class UserApiKeyRole
{
    public UserApiKeyRole()
    {
    }

    public UserApiKeyRole(Guid userApiKeyId, Guid roleId)
    {
        this.UserApiKeyId = userApiKeyId;
        this.RoleId = roleId;
    }

    public Guid UserApiKeyId { get; set; }

    public Guid RoleId { get; set; }

    public virtual UserApiKey UserApiKey { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}