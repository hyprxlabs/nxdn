using Hyprx.AspNetCore.Identity;

namespace Hyprship.Data.Models;

public class UserLoginProvider : IdentityUserLoginProvider<Guid>
{
    public UserLoginProvider()
    {
    }
    
    public UserLoginProvider(Guid userId)
    {
        this.UserId = userId;
    }

    public UserLoginProvider(Guid userId, string loginProvider, string providerKey)
    {
        this.UserId = userId;
        this.LoginProvider = loginProvider;
        this.ProviderKey = providerKey;
    }

    public virtual User User { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}