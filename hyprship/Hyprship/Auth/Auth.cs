using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using System.Text;

using Hyprship.Data.Models;
using Hyprship.Lib;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Hyprship.Auth;

public static class Auth
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapGet("auth/user/login",  Login);
    }

    public static async Task<IResult> Login(
        [FromBody] LoginRequest req,
        [FromQuery] bool useCookies,
        [FromQuery] bool useSessionCookies,
        [FromServices] IConfiguration configuration,
        [FromServices] IServiceProvider sp,
        [FromServices] ILogger logger)
    {
        var res = new LoginResponse();
        var signInManager = sp.GetRequiredService<SignInManager<User>>();
        var secretKey = configuration.GetValue<string>("jwt:secret-key");
        var expirationDurationString = configuration.GetValue<string>("jwt:duration");
        if (string.IsNullOrWhiteSpace(secretKey))
        {
            logger.LogCritical("missing jwt:secret-key configuration");
            return res.ServerError(new("server jwt misconfiguration"));
        }

        if (!TimeSpan.TryParse(expirationDurationString, out var expirationDuration))
        {
            logger.LogWarning("invalid jwt:duration configuration, defaulting to 1 hour");
            expirationDuration = TimeSpan.FromHours(1);
        }

        var useCookieScheme = (useCookies == true) || (useSessionCookies == true);
        var isPersistent = (useCookies == true) && (useSessionCookies != true);
        signInManager.AuthenticationScheme = useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme; 

        var result = await signInManager.PasswordSignInAsync(req.Email, req.Password, isPersistent, lockoutOnFailure: true);

        if (result.RequiresTwoFactor)
        {
            if (!string.IsNullOrEmpty(req.TwoFactorCode))
            {
                result = await signInManager.TwoFactorAuthenticatorSignInAsync(req.TwoFactorCode, isPersistent, rememberClient: isPersistent);
            }
            else if (!string.IsNullOrEmpty(req.TwoFactorRecoveryCode))
            {
                result = await signInManager.TwoFactorRecoveryCodeSignInAsync(req.TwoFactorRecoveryCode);
            }
        }

        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
                return res.Fail(new("User account is locked out"), StatusCodes.Status401Unauthorized);
            if (result.IsNotAllowed)
                return res.Fail(new("User is not allowed"), StatusCodes.Status401Unauthorized);
        }

        // sign in manager should handle jwt or cookie setting
        return TypedResults.Empty;
    }

    public static async Task<IResult> Logout(
        ClaimsPrincipal claimsPrincipal,
        [FromServices] ILogger logger,
        [FromServices] IServiceProvider sp)
    {
        var userManager = sp.GetRequiredService<UserManager<User>>();
        var user = await userManager.GetUserAsync(claimsPrincipal);
        if (user == null)
            return TypedResults.Unauthorized();

        logger.LogInformation("user {Email} - {UserId} logging out", user.Id, user.Email);

        var signInManager = sp.GetRequiredService<SignInManager<User>>();
        await signInManager.SignOutAsync();
        return TypedResults.Ok();
    }
}


public class LoginRequest
{
    public string Email { get; set; }

    public string Password { get; set; }

    public string? TwoFactorCode { get; set; }

    public string? TwoFactorRecoveryCode { get; set; }
}

public record LoginResult(string Token);

public class LoginResponse : ApiResult<LoginResponse, LoginResult>
{
}