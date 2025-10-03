namespace Hyprx.AspNetCore.Identity;

public class IdentityUserApiKey : IdentityUserApiKey<string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityUserApiKey"/> class.
    /// </summary>
    /// <remarks>
    /// The Id property is initialized to form a new GUID string value.
    /// </remarks>
    public IdentityUserApiKey()
    {
        this.Id = Guid.CreateVersion7().ToString();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityUserApiKey"/> class.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <remarks>
    /// The Id property is initialized to form a new GUID string value.
    /// </remarks>
    public IdentityUserApiKey(string userId)
        : this()
    {
        this.UserId = userId;
    }
}

public class IdentityUserApiKey<TKey>
      where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityUserApiKey{TKey}"/> class.
    /// </summary>
    public IdentityUserApiKey()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityUserApiKey{TKey}"/> class.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    public IdentityUserApiKey(TKey id)
        : this()
    {
        this.UserId = id;
    }

    public virtual TKey Id { get; set; } = default!;

    public virtual TKey UserId { get; set; } = default!;

    public string Name { get; set; } = default!;

    public virtual string KeyDigest { get; set; } = default!;

    public DateTime? ExpiresAt { get; set; }
}