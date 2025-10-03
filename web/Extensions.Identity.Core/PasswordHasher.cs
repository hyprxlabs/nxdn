using System.Buffers.Binary;
using System.Security.Cryptography;

using Hyprx.Crypto;

using Microsoft.AspNetCore.Identity;

namespace Hyprx.AspNetCore.Identity;

public class PasswordHasher : IPasswordHasher
{
    private const ushort Id = 1;
    private const ushort Version = 1;

    private readonly PasswordHasherOptions options;

    public PasswordHasher(PasswordHasherOptions? options = null)
    {
        this.options = options ?? new PasswordHasherOptions();
    }

    public string HashPassword(string password)
    {
        ArgumentNullException.ThrowIfNull(password);
        var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
        var hashBytes = this.ComputeHash(passwordBytes);
        return Convert.ToBase64String(hashBytes);
    }

    public byte[] ComputeHash(byte[] passwordBytes)
    {
        var salt = new byte[this.options.SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        var key = ComputeHash(passwordBytes, salt, this.options.IterationCount, this.options.HashSize, Pbkdf2Hash.SHA512);

        var result = new byte[2 + 2 + 2 + 2 + 4 + salt.Length + key.Length];
        BinaryPrimitives.WriteUInt16BigEndian(result.AsSpan(0, 2), Id);
        BinaryPrimitives.WriteUInt16BigEndian(result.AsSpan(2, 2), Version);
        BinaryPrimitives.WriteUInt16BigEndian(result.AsSpan(4, 2), (ushort)Pbkdf2Hash.SHA512.Id);
        BinaryPrimitives.WriteUInt16BigEndian(result.AsSpan(6, 2), (ushort)salt.Length);
        BinaryPrimitives.WriteUInt32BigEndian(result.AsSpan(8, 4), (uint)this.options.IterationCount);
        salt.AsSpan().CopyTo(result.AsSpan(12, salt.Length));
        key.AsSpan().CopyTo(result.AsSpan(12 + salt.Length, key.Length));
        Array.Clear(salt, 0, salt.Length);
        Array.Clear(key, 0, key.Length);
        return result;
    }

    public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        ArgumentNullException.ThrowIfNull(hashedPassword);
        ArgumentNullException.ThrowIfNull(providedPassword);

        var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
        var providedPasswordBytes = System.Text.Encoding.UTF8.GetBytes(providedPassword);
        return this.VerifyHash(hashedPasswordBytes, providedPasswordBytes) ?
            PasswordVerificationResult.Success :
            PasswordVerificationResult.Failed;
    }

    public bool VerifyHash(byte[] hashedPassword, byte[] providedPasswordBytes)
    {
        var id = BinaryPrimitives.ReadUInt16BigEndian(hashedPassword.AsSpan(0, 2));
        if (id != Id)
        {
            return false;
        }

        var version = BinaryPrimitives.ReadUInt16BigEndian(hashedPassword.AsSpan(2, 2));
        if (version != Version)
        {
            return false;
        }

        var hashAlgoId = BinaryPrimitives.ReadUInt16BigEndian(hashedPassword.AsSpan(4, 2));
        var saltSize = BinaryPrimitives.ReadUInt16BigEndian(hashedPassword.AsSpan(6, 2));
        var iterationCount = BinaryPrimitives.ReadUInt32BigEndian(hashedPassword.AsSpan(8, 4));

        var hashAlgo = Pbkdf2Hash.FromId((short)hashAlgoId);
        var salt = hashedPassword.AsSpan(12, saltSize).ToArray();
        var storedKey = hashedPassword.AsSpan(12 + saltSize).ToArray();

        var computedKey = ComputeHash(providedPasswordBytes, salt, (int)iterationCount, storedKey.Length, hashAlgo);
        Array.Clear(salt, 0, salt.Length);
        return CryptographicOperations.FixedTimeEquals(storedKey, computedKey);
    }

    private static byte[] ComputeHash(byte[] passwordBytes, byte[] salt, int iterationCount, int hashSize, Pbkdf2Hash hashAlgo)
    {
        return Rfc2898DeriveBytes.Pbkdf2(
            passwordBytes,
            salt,
            iterationCount,
            hashAlgo,
            hashSize);
    }
}

public class PasswordHasherOptions
{
    public int IterationCount { get; set; } = 100000;

    public int SaltSize { get; set; } = 16;

    public int HashSize { get; set; } = 32;
}