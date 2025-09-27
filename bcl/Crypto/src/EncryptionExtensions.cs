﻿using System.Text;

namespace Hyprx.Crypto;

public static class EncryptionExtensions
{
    public static bool SlowEquals(this IList<byte> left, IList<byte> right)
    {
        var l = Math.Min(left.Count, right.Count);
        uint diff = (uint)left.Count ^ (uint)right.Count;
        for (int i = 0; i < l; i++)
        {
            diff |= (uint)(left[i] ^ right[i]);
        }

        return diff == 0;
    }

    public static bool SlowEquals(this Span<byte> left, Span<byte> right)
    {
        var l = Math.Min(left.Length, right.Length);
        uint diff = (uint)left.Length ^ (uint)right.Length;
        for (int i = 0; i < l; i++)
        {
            diff |= (uint)(left[i] ^ right[i]);
        }

        return diff == 0;
    }

    public static bool SlowEquals(this ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
    {
        var l = Math.Min(left.Length, right.Length);
        uint diff = (uint)left.Length ^ (uint)right.Length;
        for (int i = 0; i < l; i++)
        {
            diff |= (uint)(left[i] ^ right[i]);
        }

        return diff == 0;
    }

    public static string Encrypt(this IEncryptionProvider provider, string data, Encoding? encoding = null)
    {
        encoding ??= EncryptionUtil.DefaultEncoding;
        var bytes = encoding.GetBytes(data);

        var encrypted = provider.Encrypt(bytes);
        var base64 = Convert.ToBase64String(encrypted);
        Array.Clear(bytes, 0, bytes.Length);
        Array.Clear(encrypted, 0, encrypted.Length);

        return base64;
    }

    public static string Decrypt(this IEncryptionProvider provider, string encryptedData, Encoding? encoding = null)
    {
        encoding ??= EncryptionUtil.DefaultEncoding;
        var bytes = Convert.FromBase64String(encryptedData);

        var decrypted = provider.Decrypt(bytes);
        var data = encoding.GetString(decrypted);
        Array.Clear(bytes, 0, bytes.Length);
        Array.Clear(decrypted, 0, decrypted.Length);

        return data;
    }
}