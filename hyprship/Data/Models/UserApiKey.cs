namespace Hyprship.Data.Models;

public class UserApiKey : IdentityUserApiKey<Guid>
{
    public UserApiKey()
    {
        this.Id = Guid.CreateVersion7();
        this.CreatedAt = DateTime.UtcNow;
    }
}

public class IdentityUserApiKey<TKey>
where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; } = default!;

    public TKey UserId { get; set; } = default!;

    public virtual User User { get; set; } = null!;

    public virtual HashSet<UserApiKeyRole> Roles { get; set; } = new();

    public string Name { get; set; } = null!;

    public string KeyDigest { get; set; } = null!;

    public DateTime? ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}