using Microsoft.AspNetCore.Identity;

namespace Hypership.Services.Identity;

public class PasswordHasher<TUser> : PasswordHasher, Microsoft.AspNetCore.Identity.IPasswordHasher<TUser>
    where TUser : class
{
    /// <summary>
    /// Returns a hashed representation of the supplied <paramref name="password"/> for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose password is to be hashed.</param>
    /// <param name="password">The password to hash.</param>
    /// <returns>A hashed representation of the supplied <paramref name="password"/> for the specified <paramref name="user"/>.</returns>
    public string HashPassword(TUser user, string password)
        => this.HashPassword(password);

    /// <summary>
    /// Returns a <see cref="PasswordVerificationResult"/> indicating the result of a password hash comparison.
    /// </summary>
    /// <param name="user">The user whose password should be verified.</param>
    /// <param name="hashedPassword">The hash value for a user's stored password.</param>
    /// <param name="providedPassword">The password supplied for comparison.</param>
    /// <returns>A <see cref="PasswordVerificationResult"/> indicating the result of a password hash comparison.</returns>
    /// <remarks>Implementations of this method should be time consistent.</remarks>
    public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
       => this.VerifyHashedPassword(hashedPassword, providedPassword);
}