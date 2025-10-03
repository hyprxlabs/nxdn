namespace Hyprx.AspNetCore.Identity;

public class IdentityUserApiKeyRole<TKey>
      where TKey : IEquatable<TKey>
{
    public virtual TKey UserApiKeyId { get; set; } = default!;

    public virtual TKey RoleId { get; set; } = default!;
}