using Hyprx.AspNetCore.Identity;

using Microsoft.AspNetCore.Identity;

namespace Hyprship.Data.Models;

public class UserLoginProviderToken
{
    public UserLoginProviderToken()
    {
    }

    public UserLoginProviderToken(Guid userId)
    {
        this.UserId = userId;
    }

    /// <summary>
    /// Gets or sets the primary key of the user that the token belongs to.
    /// </summary>
    public virtual Guid UserId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the LoginProvider this token is from.
    /// </summary>
    public virtual string LoginProvider { get; set; } = default!;

    /// <summary>
    /// Gets or sets the name of the token.
    /// </summary>
    public virtual string Name { get; set; } = default!;

    /// <summary>
    /// Gets or sets the token value.
    /// </summary>
    [ProtectedPersonalData]
    public virtual string? Value { get; set; }

    /// <summary>
    /// Gets or sets the user associated with the token.
    /// </summary>
    public virtual User? User { get; set; } = null;
}