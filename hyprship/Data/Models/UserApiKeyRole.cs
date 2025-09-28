namespace Hyprship.Data.Models;

public class UserApiKeyRole
{
    public Guid UserApiKeyId { get; set; }

    public Guid RoleId { get; set; }

    public virtual UserApiKey UserApiKey { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}