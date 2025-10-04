using System.Text.Json;

using Hyprship.Lib;

namespace Hyprship.Routes;

public sealed class InfoResponse
{
    /// <summary>
    /// Gets the email address associated with the authenticated user.
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// Gets a value indicating whether the email is verified.
    /// </summary>
    public required bool IsEmailVerified { get; init; }
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string? TwoFactorCode { get; set; }

    public string? TwoFactorRecoveryCode { get; set; }
}

public class RegistrationRequest
{
    public string Email { get; set; } = string.Empty;

    public string? UserName { get; set; }

    public string Password { get; set; } = string.Empty;
}

public record LoginResult(string Token);

public class LoginResponse : ApiResult<LoginResponse, LoginResult>
{
}

public sealed class AccessTokenResponse
{
    /// <summary>
    /// Gets the value. The value is always "Bearer" which indicates this response provides a "Bearer" token
    /// in the form of an opaque <see cref="AccessToken"/>.
    /// </summary>
    /// <remarks>
    /// This is serialized as "tokenType": "Bearer" using <see cref="JsonSerializerDefaults.Web"/>.
    /// </remarks>
    public string TokenType { get; } = "Bearer";

    /// <summary>
    /// Gets the opaque bearer token to send as part of the Authorization request header.
    /// </summary>
    /// <remarks>
    /// This is serialized as "accessToken": "{AccessToken}" using <see cref="JsonSerializerDefaults.Web"/>.
    /// </remarks>
    public required string AccessToken { get; init; }

    /// <summary>
    /// Gets the number of seconds before the <see cref="AccessToken"/> expires.
    /// </summary>
    /// <remarks>
    /// This is serialized as "expiresIn": "{ExpiresInSeconds}" using <see cref="JsonSerializerDefaults.Web"/>.
    /// </remarks>
    public required long ExpiresIn { get; init; }

    /// <summary>
    /// Gets the refresh token. If set, this provides the ability to get a
    /// new access_token after it expires using a refresh endpoint.
    /// </summary>
    /// <remarks>
    /// This is serialized as "refreshToken": "{RefreshToken}" using <see cref="JsonSerializerDefaults.Web"/>.
    /// </remarks>
    public required string RefreshToken { get; init; }
}