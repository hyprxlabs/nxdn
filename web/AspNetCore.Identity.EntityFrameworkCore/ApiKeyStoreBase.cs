using Microsoft.AspNetCore.Identity;

namespace Hyprx.AspNetCore.Identity;

public abstract class ApiKeyStoreBase<TUser, TRole, TKey, TUserApiKey, TRoleClaim, TUserApiKeyRole> :
    IDisposable
    where TUser : IdentityUser<TKey>
    where TKey : IEquatable<TKey>
    where TUserApiKey : IdentityUserApiKey<TKey>, new()
    where TRole : IdentityRole<TKey>
    where TRoleClaim : IdentityRoleClaim<TKey>, new()
    where TUserApiKeyRole : IdentityUserApiKeyRole<TKey>, new()
{
    private bool disposed;

    public ApiKeyStoreBase(IdentityErrorDescriber describer)
    {
        ArgumentNullException.ThrowIfNull(describer);
        this.ErrorDescriber = describer;
    }

    public abstract IQueryable<TUserApiKey> ApiKeys { get; }

    private IdentityErrorDescriber ErrorDescriber { get; }

    public abstract Task<TUserApiKey?> FindByKeyAsync(string key);

    public abstract Task<IList<string>> GetApiKeyRolesAsync(TUserApiKey apiKey);

    public abstract Task<TUser> GetApiKeyUserAsync(TUserApiKey apiKey, CancellationToken cancellationToken);

    public Task SetApiKeyExpirationAsync(TUserApiKey apiKey, DateTimeOffset? expiration)
    {
        apiKey.ExpiresAt = expiration.HasValue ? expiration.Value.UtcDateTime : null;
        return Task.CompletedTask;
    }

    public abstract Task AddToRoleAsync(TUserApiKey apiKey, string roleName);

    public abstract Task RemoveFromRoleAsync(TUserApiKey apiKey, string roleName);

    public abstract Task<TRole> FindRoleAsync(string roleName, CancellationToken cancellationToken);

    public virtual void Dispose()
       => this.disposed = true;

    protected virtual TUserApiKeyRole CreateUserApiKeyRole(TKey apiKeyId, TKey roleId)
        => new TUserApiKeyRole()
        {
            UserApiKeyId = apiKeyId,
            RoleId = roleId,
        };

    protected virtual TUserApiKey CreateUserApiKey(TUser user, string keyDigest)
        => new TUserApiKey()
        {
            UserId = user.Id,
            KeyDigest = keyDigest,
        };

    protected void ThrowIfDisposed()
        => _ = this.disposed ? throw new ObjectDisposedException(this.GetType().Name) : 0;
}