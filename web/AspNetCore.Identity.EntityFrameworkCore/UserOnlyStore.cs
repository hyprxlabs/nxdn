// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Security.Claims;

using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;

namespace Hyprx.AspNetCore.Identity.EntityFrameworkCore;

/// <summary>
/// Creates a new instance of a persistence store for the specified user type.
/// </summary>
/// <typeparam name="TUser">The type representing a user.</typeparam>
public class UserOnlyStore<TUser> : UserOnlyStore<TUser, DbContext, Guid>
    where TUser : IdentityUser<Guid>, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserOnlyStore{TUser}"/> class.
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/>.</param>
    /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
    public UserOnlyStore(DbContext context, IdentityErrorDescriber? describer = null)
        : base(context, describer)
    {
    }
}

/// <summary>
/// Represents a new instance of a persistence store for the specified user and role types.
/// </summary>
/// <typeparam name="TUser">The type representing a user.</typeparam>
/// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
public class UserOnlyStore<TUser, TContext> : UserOnlyStore<TUser, TContext, Guid>
    where TUser : IdentityUser<Guid>
    where TContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserOnlyStore{TUser, TContext}"/> class.
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/>.</param>
    /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
    public UserOnlyStore(TContext context, IdentityErrorDescriber? describer = null)
        : base(context, describer)
    {
    }
}

/// <summary>
/// Represents a new instance of a persistence store for the specified user and role types.
/// </summary>
/// <typeparam name="TUser">The type representing a user.</typeparam>
/// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
/// <typeparam name="TKey">The type of the primary key for a user.</typeparam>
public class UserOnlyStore<TUser, TContext, TKey> : UserOnlyStore<TUser, TContext, TKey, IdentityUserClaim<TKey>, IdentityUserPasswordAuth<TKey>, IdentityUserLoginProvider<TKey>, IdentityUserToken<TKey>>
    where TUser : IdentityUser<TKey>
    where TContext : DbContext
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserOnlyStore{TUser, TContext, TKey}"/> class.
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/>.</param>
    /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
    public UserOnlyStore(TContext context, IdentityErrorDescriber? describer = null)
        : base(context, describer)
    {
    }
}

/// <summary>
/// Represents a new instance of a persistence store for the specified user and role types.
/// </summary>
/// <typeparam name="TUser">The type representing a user.</typeparam>
/// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
/// <typeparam name="TKey">The type of the primary key for a user.</typeparam>
/// <typeparam name="TUserClaim">The type representing a claim.</typeparam>
/// <typeparam name="TUserPasswordAuth">The type representing a user's password authentication information.</typeparam>
/// <typeparam name="TUserLogin">The type representing a user external login.</typeparam>
/// <typeparam name="TUserToken">The type representing a user token.</typeparam>
public class UserOnlyStore<TUser, TContext, TKey, TUserClaim, TUserPasswordAuth, TUserLogin, TUserToken> :
    UserOnlyStore<TUser, TContext, TKey, TUserClaim, TUserPasswordAuth, TUserLogin, TUserToken, IdentityUserPasskey<TKey>>
    where TUser : IdentityUser<TKey>
    where TContext : DbContext
    where TKey : IEquatable<TKey>
    where TUserClaim : IdentityUserClaim<TKey>, new()
    where TUserPasswordAuth : IdentityUserPasswordAuth<TKey>, new()
    where TUserLogin : IdentityUserLoginProvider<TKey>, new()
    where TUserToken : IdentityUserToken<TKey>, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserOnlyStore{TUser, TContext, TKey, TUserClaim, TUserPasswordAuth, TUserLogin, TUserToken}"/> class.
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/>.</param>
    /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
    public UserOnlyStore(TContext context, IdentityErrorDescriber? describer = null)
        : base(context, describer)
    {
    }
}

/// <summary>
/// Represents a new instance of a persistence store for the specified user and role types.
/// </summary>
/// <typeparam name="TUser">The type representing a user.</typeparam>
/// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
/// <typeparam name="TKey">The type of the primary key for a user.</typeparam>
/// <typeparam name="TUserClaim">The type representing a claim.</typeparam>
/// <typeparam name="TUserPasswordAuth">The type representing a user's password authentication information.</typeparam>
/// <typeparam name="TUserLogin">The type representing a user external login.</typeparam>
/// <typeparam name="TUserToken">The type representing a user token.</typeparam>
/// <typeparam name="TUserPasskey">The type representing a user passkey.</typeparam>
public class UserOnlyStore<TUser, TContext, TKey, TUserClaim, TUserPasswordAuth, TUserLogin, TUserToken, TUserPasskey> :
    UserStoreBase<TUser, TKey, TUserClaim, TUserPasswordAuth, TUserLogin, TUserToken>,
    IUserLoginStore<TUser>,
    IUserClaimStore<TUser>,
    IUserPasswordStore<TUser>,
    IUserSecurityStampStore<TUser>,
    IUserEmailStore<TUser>,
    IUserLockoutStore<TUser>,
    IUserPhoneNumberStore<TUser>,
    IQueryableUserStore<TUser>,
    IUserTwoFactorStore<TUser>,
    IUserAuthenticationTokenStore<TUser>,
    IUserAuthenticatorKeyStore<TUser>,
    IUserTwoFactorRecoveryCodeStore<TUser>,
    IProtectedUserStore<TUser>,
    IUserPasskeyStore<TUser>
    where TUser : IdentityUser<TKey>
    where TContext : DbContext
    where TKey : IEquatable<TKey>
    where TUserClaim : IdentityUserClaim<TKey>, new()
    where TUserPasswordAuth : IdentityUserPasswordAuth<TKey>, new()
    where TUserLogin : IdentityUserLoginProvider<TKey>, new()
    where TUserToken : IdentityUserToken<TKey>, new()
    where TUserPasskey : IdentityUserPasskey<TKey>, new()
{
    private bool? dbContextSupportsPasskeys;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserOnlyStore{TUser, TContext, TKey, TUserClaim, TUserPasswordAuth, TUserLogin, TUserToken, TUserPasskey}"/> class.
    /// </summary>
    /// <param name="context">The context used to access the store.</param>
    /// <param name="describer">The <see cref="IdentityErrorDescriber"/> used to describe store errors.</param>
    public UserOnlyStore(TContext context, IdentityErrorDescriber? describer = null)
        : base(describer ?? new IdentityErrorDescriber())
    {
        ArgumentNullException.ThrowIfNull(context);
        this.Context = context;
    }

    /// <summary>
    /// Gets the navigation property for the users the store contains.
    /// </summary>
    public override IQueryable<TUser> Users
    {
        get { return this.UsersSet; }
    }

    /// <summary>
    /// Gets the database context for this store.
    /// </summary>
    public virtual TContext Context { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets a flag indicating if changes should be persisted after CreateAsync, UpdateAsync and DeleteAsync are called.
    /// </summary>
    /// <value>
    /// True if changes should be automatically persisted, otherwise false.
    /// </value>
    public bool AutoSaveChanges { get; set; } = true;

    /// <summary>
    /// Gets the <see cref="DbSet{TUser}"/> of users.
    /// </summary>
    protected DbSet<TUser> UsersSet => this.Context.Set<TUser>();

    /// <summary>
    /// Gets the <see cref="DbSet{TUserClaim}"/> of user claims.
    /// </summary>
    protected DbSet<TUserClaim> UserClaims => this.Context.Set<TUserClaim>();

    /// <summary>
    /// Gets the <see cref="DbSet{TUserLogin}"/> of user logins.
    /// </summary>
    protected DbSet<TUserLogin> UserLogins => this.Context.Set<TUserLogin>();

    /// <summary>
    /// Gets the <see cref="DbSet{TUserToken}"/> of user tokens.
    /// </summary>
    protected DbSet<TUserToken> UserTokens => this.Context.Set<TUserToken>();

    /// <summary>
    /// Gets the <see cref="DbSet{TUserPasswordAuth}"/> of user password authentications.
    /// </summary>
    protected DbSet<TUserPasswordAuth> UserPasswordAuths => this.Context.Set<TUserPasswordAuth>();

    /// <summary>
    /// Gets the <see cref="DbSet{TUserPasskey}"/> of user passkeys.
    /// </summary>
    protected DbSet<TUserPasskey> UserPasskeys
    {
        get
        {
            this.ThrowIfPasskeysNotSupported();
            return this.Context.Set<TUserPasskey>();
        }
    }

    /// <summary>
    /// Creates the specified <paramref name="user"/> in the user store.
    /// </summary>
    /// <param name="user">The user to create.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the creation operation.</returns>
    public override async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        this.ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        this.Context.Add(user);
        await this.SaveChanges(cancellationToken);
        return IdentityResult.Success;
    }

    /// <summary>
    /// Updates the specified <paramref name="user"/> in the user store.
    /// </summary>
    /// <param name="user">The user to update.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the update operation.</returns>
    public override async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        this.ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);

        this.Context.Attach(user);
        user.ConcurrencyStamp = Guid.NewGuid().ToString();
        this.Context.Update(user);
        try
        {
            await this.SaveChanges(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return IdentityResult.Failed(this.ErrorDescriber.ConcurrencyFailure());
        }

        return IdentityResult.Success;
    }

    /// <summary>
    /// Deletes the specified <paramref name="user"/> from the user store.
    /// </summary>
    /// <param name="user">The user to delete.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the update operation.</returns>
    public override async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        this.ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);

        this.Context.Remove(user);
        try
        {
            await this.SaveChanges(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return IdentityResult.Failed(this.ErrorDescriber.ConcurrencyFailure());
        }

        return IdentityResult.Success;
    }

    /// <summary>
    /// Finds and returns a user, if any, who has the specified <paramref name="userId"/>.
    /// </summary>
    /// <param name="userId">The user ID to search for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="userId"/> if it exists.
    /// </returns>
    public override Task<TUser?> FindByIdAsync(string userId, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        this.ThrowIfDisposed();
        var id = this.ConvertIdFromString(userId);
        return this.UsersSet.FindAsync(new object?[] { id }, cancellationToken).AsTask();
    }

    /// <summary>
    /// Finds and returns a user, if any, who has the specified normalized user name.
    /// </summary>
    /// <param name="normalizedUserName">The normalized user name to search for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="normalizedUserName"/> if it exists.
    /// </returns>
    public override Task<TUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        this.ThrowIfDisposed();

        return this.Users.FirstOrDefaultAsync(u => u.UserName == normalizedUserName, cancellationToken);
    }

    /// <summary>
    /// Get the claims associated with the specified <paramref name="user"/> as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user whose claims should be retrieved.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that contains the claims granted to a user.</returns>
    public override async Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
    {
        this.ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);

        return await this.UserClaims.Where(uc => uc.UserId.Equals(user.Id)).Select(c => c.ToClaim()).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Adds the <paramref name="claims"/> given to the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user to add the claim to.</param>
    /// <param name="claims">The claim to add to the user.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public override Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default(CancellationToken))
    {
        this.ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(claims);
        foreach (var claim in claims)
        {
            this.UserClaims.Add(this.CreateUserClaim(user, claim));
        }

        return Task.FromResult(false);
    }

    /// <summary>
    /// Replaces the <paramref name="claim"/> on the specified <paramref name="user"/>, with the <paramref name="newClaim"/>.
    /// </summary>
    /// <param name="user">The user to replace the claim on.</param>
    /// <param name="claim">The claim replace.</param>
    /// <param name="newClaim">The new claim replacing the <paramref name="claim"/>.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public override async Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default(CancellationToken))
    {
        this.ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(claim);
        ArgumentNullException.ThrowIfNull(newClaim);

        var matchedClaims = await this.UserClaims
            .Where(uc => uc.UserId.Equals(user.Id) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type)
            .ToListAsync(cancellationToken);
        foreach (var matchedClaim in matchedClaims)
        {
            matchedClaim.ClaimValue = newClaim.Value;
            matchedClaim.ClaimType = newClaim.Type;
        }
    }

    /// <summary>
    /// Removes the <paramref name="claims"/> given from the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user to remove the claims from.</param>
    /// <param name="claims">The claim to remove.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public override async Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default(CancellationToken))
    {
        this.ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(claims);
        foreach (var claim in claims)
        {
            var matchedClaims = await this.UserClaims.Where(uc => uc.UserId.Equals(user.Id) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type).ToListAsync(cancellationToken);
            foreach (var c in matchedClaims)
            {
                this.UserClaims.Remove(c);
            }
        }
    }

    /// <summary>
    /// Adds the <paramref name="login"/> given to the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user to add the login to.</param>
    /// <param name="login">The login to add to the user.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public override Task AddLoginAsync(
        TUser user,
        UserLoginInfo login,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        this.ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(login);
        this.UserLogins.Add(this.CreateUserLogin(user, login));
        return Task.FromResult(false);
    }

    /// <summary>
    /// Removes the <paramref name="loginProvider"/> given from the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user to remove the login from.</param>
    /// <param name="loginProvider">The login to remove from the user.</param>
    /// <param name="providerKey">The key provided by the <paramref name="loginProvider"/> to identify a user.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public override async Task RemoveLoginAsync(
        TUser user,
        string loginProvider,
        string providerKey,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        this.ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        var entry = await this.FindUserLoginAsync(user.Id, loginProvider, providerKey, cancellationToken);
        if (entry != null)
        {
            this.UserLogins.Remove(entry);
        }
    }

    /// <summary>
    /// Retrieves the associated logins for the specified <param ref="user"/>.
    /// </summary>
    /// <param name="user">The user whose associated logins to retrieve.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> for the asynchronous operation, containing a list of <see cref="UserLoginInfo"/> for the specified <paramref name="user"/>, if any.
    /// </returns>
    public override async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        this.ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        var userId = user.Id;
        return await this.UserLogins.Where(l => l.UserId.Equals(userId))
            .Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey, l.ProviderDisplayName)).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves the user associated with the specified login provider and login provider key.
    /// </summary>
    /// <param name="loginProvider">The login provider who provided the <paramref name="providerKey"/>.</param>
    /// <param name="providerKey">The key provided by the <paramref name="loginProvider"/> to identify a user.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> for the asynchronous operation, containing the user, if any which matched the specified login provider and key.
    /// </returns>
    public override async Task<TUser?> FindByLoginAsync(
        string loginProvider,
        string providerKey,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        this.ThrowIfDisposed();
        var userLogin = await this.FindUserLoginAsync(loginProvider, providerKey, cancellationToken);
        if (userLogin != null)
        {
            return await this.FindUserAsync(userLogin.UserId, cancellationToken);
        }

        return null;
    }

    /// <summary>
    /// Gets the user, if any, associated with the specified, normalized email address.
    /// </summary>
    /// <param name="normalizedEmail">The normalized email address to return the user for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The task object containing the results of the asynchronous lookup operation, the user if any associated with the specified normalized email address.
    /// </returns>
    public override Task<TUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        this.ThrowIfDisposed();

        return this.Users.SingleOrDefaultAsync(u => u.Email == normalizedEmail, cancellationToken);
    }

    /// <summary>
    /// Retrieves all users with the specified claim.
    /// </summary>
    /// <param name="claim">The claim whose users should be retrieved.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> contains a list of users, if any, that contain the specified claim.
    /// </returns>
    public override async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        this.ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(claim);

        var query = from userclaims in this.UserClaims
                    join user in this.Users on userclaims.UserId equals user.Id
                    where userclaims.ClaimValue == claim.Value
                    && userclaims.ClaimType == claim.Type
                    select user;

        return await query.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Creates a new passkey credential in the store for the specified <paramref name="user"/>,
    /// or updates an existing passkey.
    /// </summary>
    /// <param name="user">The user to create the passkey credential for.</param>
    /// <param name="passkey">The passkey information.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public virtual async Task AddOrUpdatePasskeyAsync(TUser user, UserPasskeyInfo passkey, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        this.ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(passkey);

        var userPasskey = await this.FindUserPasskeyByIdAsync(passkey.CredentialId, cancellationToken).ConfigureAwait(false);
        if (userPasskey != null)
        {
            userPasskey.Data.Name = passkey.Name;
            userPasskey.Data.SignCount = passkey.SignCount;
            userPasskey.Data.IsBackedUp = passkey.IsBackedUp;
            userPasskey.Data.IsUserVerified = passkey.IsUserVerified;
            this.UserPasskeys.Update(userPasskey);
        }
        else
        {
            userPasskey = this.CreateUserPasskey(user, passkey);
            this.UserPasskeys.Add(userPasskey);
        }

        await this.SaveChanges(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the passkey credentials for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose passkeys should be retrieved.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing a list of the user's passkeys.</returns>
    public virtual async Task<IList<UserPasskeyInfo>> GetPasskeysAsync(TUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        this.ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);

        var userId = user.Id;
        var passkeys = await this.UserPasskeys
            .Where(p => p.UserId.Equals(userId))
            .Select(p => new UserPasskeyInfo(
                p.CredentialId,
                p.Data.PublicKey,
                p.Data.CreatedAt,
                p.Data.SignCount,
                p.Data.Transports,
                p.Data.IsUserVerified,
                p.Data.IsBackupEligible,
                p.Data.IsBackedUp,
                p.Data.AttestationObject,
                p.Data.ClientDataJson)
            {
                Name = p.Data.Name,
            })
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return passkeys;
    }

    /// <summary>
    /// Finds and returns a user, if any, associated with the specified passkey credential identifier.
    /// </summary>
    /// <param name="credentialId">The passkey credential id to search for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the user, if any, associated with the specified passkey credential id.
    /// </returns>
    public virtual async Task<TUser?> FindByPasskeyIdAsync(byte[] credentialId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        this.ThrowIfDisposed();

        var passkey = await this.FindUserPasskeyByIdAsync(credentialId, cancellationToken).ConfigureAwait(false);
        if (passkey != null)
        {
            return await this.FindUserAsync(passkey.UserId, cancellationToken).ConfigureAwait(false);
        }

        return null;
    }

    /// <summary>
    /// Finds a passkey for the specified user with the specified credential id.
    /// </summary>
    /// <param name="user">The user whose passkey should be retrieved.</param>
    /// <param name="credentialId">The credential id to search for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the user's passkey information.</returns>
    public virtual async Task<UserPasskeyInfo?> FindPasskeyAsync(TUser user, byte[] credentialId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        this.ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);

        var passkey = await this.FindUserPasskeyAsync(user.Id, credentialId, cancellationToken).ConfigureAwait(false);
        if (passkey != null)
        {
            return new UserPasskeyInfo(
                passkey.CredentialId,
                passkey.Data.PublicKey,
                passkey.Data.CreatedAt,
                passkey.Data.SignCount,
                passkey.Data.Transports,
                passkey.Data.IsUserVerified,
                passkey.Data.IsBackupEligible,
                passkey.Data.IsBackedUp,
                passkey.Data.AttestationObject,
                passkey.Data.ClientDataJson)
            {
                Name = passkey.Data.Name,
            };
        }

        return null;
    }

    /// <summary>
    /// Removes a passkey credential from the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user to remove the passkey credential from.</param>
    /// <param name="credentialId">The credential id of the passkey to remove.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public virtual async Task RemovePasskeyAsync(TUser user, byte[] credentialId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        this.ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(credentialId);

        var passkey = await this.FindUserPasskeyAsync(user.Id, credentialId, cancellationToken).ConfigureAwait(false);
        if (passkey != null)
        {
            this.UserPasskeys.Remove(passkey);
            await this.SaveChanges(cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Return a user with the matching userId if it exists.
    /// </summary>
    /// <param name="userId">The user's id.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The user if it exists.</returns>
    protected override Task<TUser?> FindUserAsync(TKey userId, CancellationToken cancellationToken)
    {
        return this.Users.SingleOrDefaultAsync(u => u.Id.Equals(userId), cancellationToken);
    }

    /// <summary>
    /// Return a user login with the matching userId, provider, providerKey if it exists.
    /// </summary>
    /// <param name="userId">The user's id.</param>
    /// <param name="loginProvider">The login provider name.</param>
    /// <param name="providerKey">The key provided by the <paramref name="loginProvider"/> to identify a user.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The user login if it exists.</returns>
    protected override Task<TUserLogin?> FindUserLoginAsync(TKey userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        return this.UserLogins
            .SingleOrDefaultAsync(
                userLogin => userLogin.UserId.Equals(userId)
                && userLogin.LoginProvider == loginProvider
                && userLogin.ProviderKey == providerKey,
                cancellationToken);
    }

    /// <summary>
    /// Return a user login with  provider, providerKey if it exists.
    /// </summary>
    /// <param name="loginProvider">The login provider name.</param>
    /// <param name="providerKey">The key provided by the <paramref name="loginProvider"/> to identify a user.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The user login if it exists.</returns>
    protected override Task<TUserLogin?> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        return this.UserLogins.SingleOrDefaultAsync(userLogin => userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey, cancellationToken);
    }

    /// <summary>Saves the current store.</summary>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    protected Task SaveChanges(CancellationToken cancellationToken)
    {
        return this.AutoSaveChanges ? this.Context.SaveChangesAsync(cancellationToken) : Task.CompletedTask;
    }

    protected override async Task<TUserPasswordAuth> GetUserPasswordAuthAsync(TUser user)
    {
        var auth = await this.UserPasswordAuths.SingleOrDefaultAsync(ua => ua.UserId.Equals(user.Id));
        if (auth == null)
        {
            auth = new TUserPasswordAuth
            { UserId = user.Id };
            this.UserPasswordAuths.Add(auth);
        }

        return auth;
    }

    /// <summary>
    /// Find a user token if it exists.
    /// </summary>
    /// <param name="user">The token owner.</param>
    /// <param name="loginProvider">The login provider for the token.</param>
    /// <param name="name">The name of the token.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The user token if it exists.</returns>
    protected override Task<TUserToken?> FindTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        => this.UserTokens.FindAsync(new object[] { user.Id, loginProvider, name }, cancellationToken).AsTask();

    /// <summary>
    /// Add a new user token.
    /// </summary>
    /// <param name="token">The token to be added.</param>
    /// <returns>The task representing the asynchronous operation.</returns>
    protected override Task AddUserTokenAsync(TUserToken token)
    {
        this.UserTokens.Add(token);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Remove a new user token.
    /// </summary>
    /// <param name="token">The token to be removed.</param>
    /// <returns>The task representing the asynchronous operation.</returns>
    protected override Task RemoveUserTokenAsync(TUserToken token)
    {
        this.UserTokens.Remove(token);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Called to create a new instance of a <see cref="IdentityUserPasskey{TKey}"/>.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="passkey">The passkey.</param>
    /// <returns>The user passkey.</returns>
    protected virtual TUserPasskey CreateUserPasskey(TUser user, UserPasskeyInfo passkey)
    {
        return new TUserPasskey
        {
            UserId = user.Id,
            CredentialId = passkey.CredentialId,
            Data = new IdentityPasskeyData()
            {
                PublicKey = passkey.PublicKey,
                Name = passkey.Name,
                CreatedAt = passkey.CreatedAt,
                Transports = passkey.Transports,
                SignCount = passkey.SignCount,
                IsUserVerified = passkey.IsUserVerified,
                IsBackupEligible = passkey.IsBackupEligible,
                IsBackedUp = passkey.IsBackedUp,
                AttestationObject = passkey.AttestationObject,
                ClientDataJson = passkey.ClientDataJson,
            },
        };
    }

    /// <summary>
    /// Find a passkey with the specified credential id for a user.
    /// </summary>
    /// <param name="userId">The user's id.</param>
    /// <param name="credentialId">The credential id to search for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The user passkey if it exists.</returns>
    private Task<TUserPasskey?> FindUserPasskeyAsync(TKey userId, byte[] credentialId, CancellationToken cancellationToken)
    {
        return this.UserPasskeys.SingleOrDefaultAsync(
            userPasskey => userPasskey.UserId.Equals(userId) && userPasskey.CredentialId.SequenceEqual(credentialId),
            cancellationToken);
    }

    /// <summary>
    /// Find a passkey with the specified credential id.
    /// </summary>
    /// <param name="credentialId">The credential id to search for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The user passkey if it exists.</returns>
    private Task<TUserPasskey?> FindUserPasskeyByIdAsync(byte[] credentialId, CancellationToken cancellationToken)
       => this.UserPasskeys.SingleOrDefaultAsync(userPasskey => userPasskey.CredentialId.SequenceEqual(credentialId), cancellationToken);

    private void ThrowIfPasskeysNotSupported()
    {
        if (this.dbContextSupportsPasskeys == true)
        {
            return;
        }

        this.dbContextSupportsPasskeys ??= this.Context.Model.FindEntityType(typeof(TUserPasskey)) is not null;
        if (this.dbContextSupportsPasskeys == false)
        {
            throw new InvalidOperationException(
                $"This operation is not permitted because the underlying '{nameof(DbContext)}' does not include '{typeof(TUserPasskey).Name}' in its model. " +
                $"When using '{nameof(IdentityDbContext)}', make sure that '{nameof(IdentityOptions)}.{nameof(IdentityOptions.Stores)}.{nameof(StoreOptions.SchemaVersion)}' " +
                $"is set to '{nameof(IdentitySchemaVersions)}.{nameof(IdentitySchemaVersions.Version3)}' or higher. " +
                $"See https://aka.ms/aspnet/passkeys for more information.");
        }
    }
}