using Microsoft.AspNetCore.Identity;

namespace Hyprx.AspNetCore.Identity;

public interface IPasswordHasher
{
    string HashPassword(string password);

    byte[] ComputeHash(byte[] passwordBytes);

    PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword);

    bool VerifyHash(byte[] hashedPassword, byte[] providedPasswordBytes);
}