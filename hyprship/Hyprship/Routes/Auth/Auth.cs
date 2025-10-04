using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

using Hyprship.Data.Models;
using Hyprship.Lib;

using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

// ReSharper disable All
namespace Hyprship.Routes;

public static partial class Auth
{
    private static readonly EmailAddressAttribute emailAddressAttribute = new();
    private static TimeProvider? timeProvider;
    private static IOptionsMonitor<BearerTokenOptions>? bearerTokenOptions;
    private static IEmailSender<User>? emailSender;
    private static LinkGenerator? linkGenerator;
    private static string? confirmEmailEndpointName = null;

    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        timeProvider = app.ServiceProvider.GetRequiredService<TimeProvider>();
        bearerTokenOptions = app.ServiceProvider.GetRequiredService<IOptionsMonitor<BearerTokenOptions>>();
        emailSender = app.ServiceProvider.GetRequiredService<IEmailSender<User>>();
        linkGenerator = app.ServiceProvider.GetRequiredService<LinkGenerator>();
        app.MapPost("accounts/login", LoginAsync);
        app.MapPost("accounts/register", RegisterAsync);
        app.MapPost("accounts/logout", LogoutAsync);
        app.MapPost("accounts/refresh", RefreshAsync);
        app.MapPost("accounts/reset-password", ResetPasswordAsync);
        app.MapPost("accounts/forgot-password", ForgotPasswordAsync);
        app.MapGet("accounts/verify-email", ConfirmEmailAsync).Add(endpointBuilder =>
        {
            var finalPattern = ((RouteEndpointBuilder)endpointBuilder).RoutePattern.RawText;
            confirmEmailEndpointName = $"{nameof(Auth)}-{finalPattern}";
            endpointBuilder.Metadata.Add(new EndpointNameMetadata(confirmEmailEndpointName));
        });
        app.MapGet("accounts/resend-email-verification", ResendConfirmationAsync);
        app.MapPost("accounts/manage/2fa", Post2fa);
        app.MapGet("accounts/manage/info", GetInfo);
        app.MapPost("accounts/manage/info", PostInfo);
    }

    private static async Task<IResult> LoginAsync(
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
        signInManager.AuthenticationScheme =
            useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

        var result =
            await signInManager.PasswordSignInAsync(req.Email, req.Password, isPersistent, lockoutOnFailure: true);

        if (result.RequiresTwoFactor)
        {
            if (!string.IsNullOrEmpty(req.TwoFactorCode))
            {
                result = await signInManager.TwoFactorAuthenticatorSignInAsync(
                    req.TwoFactorCode,
                    isPersistent,
                    rememberClient: isPersistent);
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

    private static async Task<IResult> LogoutAsync(
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

    private static async Task<Results<Ok, BadRequest<ValidationError>>> RegisterAsync(
        [FromBody] RegistrationRequest request,
        HttpContext context,
        [FromServices] IServiceProvider sp)
    {
        var userManager = sp.GetRequiredService<UserManager<User>>();
        var identityOptions = sp.GetService<IOptions<IdentityOptions>>();

        if (!userManager.SupportsUserEmail)
        {
            throw new NotSupportedException($"{nameof(RegisterAsync)} requires a user store with email support.");
        }

        var userStore = sp.GetRequiredService<IUserStore<User>>();
        var emailStore = (IUserEmailStore<User>)userStore;
        var email = request.Email;

        if (string.IsNullOrEmpty(email) || !emailAddressAttribute.IsValid(email))
        {
            return CreateValidationErrors(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email)));
        }

        var userName = request.UserName;
        if (!string.IsNullOrWhiteSpace(userName))
        {
            if (userName != email)
            {
                foreach (var c in userName)
                {
                    if (char.IsLetterOrDigit(c) || c is '_' or '.' or '-')
                        continue;

                    var error = new ValidationError("Username is invalid");
                    error.Details["username"] =
                        ["User can only contain alphanumeric characters, underscores, periods, or hyphens"];
                    return TypedResults.BadRequest(error);
                }
            }
        }
        else
        {
            userName = email;
        }

        var user = new User(userName, email);
        await userStore.SetUserNameAsync(user, email, CancellationToken.None);
        await emailStore.SetEmailAsync(user, email, CancellationToken.None);
        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return CreateValidationErrors(result);
        }

        await SendConfirmationEmailAsync(user, userManager, context, email);
        return TypedResults.Ok();
    }

    private static async
        Task<Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult, SignInHttpResult, ChallengeHttpResult>>
        RefreshAsync(
            [FromBody] RefreshRequest refreshRequest,
            [FromServices] IServiceProvider sp)
    {
        var signInManager = sp.GetRequiredService<SignInManager<User>>();
        var refreshTokenProtector = bearerTokenOptions!.Get(IdentityConstants.BearerScheme).RefreshTokenProtector;
        var refreshTicket = refreshTokenProtector.Unprotect(refreshRequest.RefreshToken);

        // Reject the /refresh attempt with a 401 if the token expired or the security stamp validation
        // ReSharper disable once ConvertTypeCheckPatternToNullCheck
        if (refreshTicket?.Properties?.ExpiresUtc is not { } expiresUtc ||
            timeProvider!.GetUtcNow() >= expiresUtc ||
            await signInManager.ValidateSecurityStampAsync(refreshTicket.Principal) is not User user)
        {
            return TypedResults.Challenge();
        }

        var newPrincipal = await signInManager.CreateUserPrincipalAsync(user);
        return TypedResults.SignIn(newPrincipal, authenticationScheme: IdentityConstants.BearerScheme);
    }

    private static async Task<Ok> ResendConfirmationAsync(
        [FromBody] ResendConfirmationEmailRequest resendRequest,
        HttpContext context,
        [FromServices] IServiceProvider sp)
    {
        var userManager = sp.GetRequiredService<UserManager<User>>();
        if (await userManager.FindByEmailAsync(resendRequest.Email) is not { } user)
        {
            return TypedResults.Ok();
        }

        await SendConfirmationEmailAsync(user, userManager, context, resendRequest.Email);
        return TypedResults.Ok();
    }

    private static async Task SendConfirmationEmailAsync(
        User user,
        UserManager<User> userManager,
        HttpContext context,
        string email,
        bool isChange = false)
    {
        if (confirmEmailEndpointName is null)
        {
            throw new NotSupportedException("No email confirmation endpoint was registered!");
        }

        var code = isChange
            ? await userManager.GenerateChangeEmailTokenAsync(user, email)
            : await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var userId = await userManager.GetUserIdAsync(user);
        var routeValues = new RouteValueDictionary() { ["userId"] = userId, ["code"] = code, };

        if (isChange)
        {
            // This is validated by the /confirmEmail endpoint on change.
            routeValues.Add("changedEmail", email);
        }

        var confirmEmailUrl = linkGenerator!.GetUriByName(context, confirmEmailEndpointName, routeValues)
                              ?? throw new NotSupportedException(
                                  $"Could not find endpoint named '{confirmEmailEndpointName}'.");

        await emailSender!.SendConfirmationLinkAsync(user, email, HtmlEncoder.Default.Encode(confirmEmailUrl));
    }

    private static BadRequest<ValidationError> CreateValidationErrors(IdentityResult result)
    {
        Debug.Assert(result != null, "Result must not be null");
        Debug.Assert(!result.Succeeded, "Result must not be Successful");
        var res = new ValidationError("One or more validation errors occurred.");
        foreach (var next in result.Errors)
        {
            if (res.Details.TryGetValue(next.Code, out var errors))
            {
                var newErrors = new string[errors.Length + 1];
                Array.Copy(errors, newErrors, errors.Length);
                newErrors[errors.Length] = next.Description;
                res.Details[next.Code] = newErrors;
            }
            else
            {
                res.Details[next.Code] = [next.Description];
            }
        }

        return TypedResults.BadRequest(res);
    }

    private static async Task<Results<Ok, BadRequest<ValidationError>>> ForgotPasswordAsync(
        [FromBody] ForgotPasswordRequest resetRequest,
        [FromServices] IServiceProvider sp)
    {
        var userManager = sp.GetRequiredService<UserManager<User>>();
        var user = await userManager.FindByEmailAsync(resetRequest.Email);

        if (user is not null && await userManager.IsEmailConfirmedAsync(user))
        {
            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            await emailSender!.SendPasswordResetCodeAsync(user, resetRequest.Email, HtmlEncoder.Default.Encode(code));
        }

        // Don't reveal that the user does not exist or is not confirmed, so don't return a 200 if we would have
        // returned a 400 for an invalid code given a valid user email.
        return TypedResults.Ok();
    }

    private static async Task<Results<Ok<InfoResponse>, ValidationProblem, NotFound>> GetInfo(
        ClaimsPrincipal claimsPrincipal, [FromServices] IServiceProvider sp)
    {
        var userManager = sp.GetRequiredService<UserManager<User>>();
        if (await userManager.GetUserAsync(claimsPrincipal) is not { } user)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(await CreateInfoResponseAsync(user, userManager));
    }

    private static async Task<Results<Ok<InfoResponse>, BadRequest<ValidationError>, NotFound>> PostInfo(
        ClaimsPrincipal claimsPrincipal,
        [FromBody] InfoRequest infoRequest,
        HttpContext context,
        [FromServices] IServiceProvider sp)
    {
        var userManager = sp.GetRequiredService<UserManager<User>>();
        if (await userManager.GetUserAsync(claimsPrincipal) is not { } user)
        {
            return TypedResults.NotFound();
        }

        if (!string.IsNullOrEmpty(infoRequest.NewEmail) && !emailAddressAttribute.IsValid(infoRequest.NewEmail))
        {
            return CreateValidationErrors(
                IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(infoRequest.NewEmail)));
        }

        if (!string.IsNullOrEmpty(infoRequest.NewPassword))
        {
            if (string.IsNullOrEmpty(infoRequest.OldPassword))
            {
                var res = new ValidationError("New password is required.");
                res.Details["Password"] =
                [
                    "The old password is required to set a new password. If the old password is forgotten, use /resetPassword."
                ];
                return TypedResults.BadRequest(res);
            }

            var changePasswordResult =
                await userManager.ChangePasswordAsync(user, infoRequest.OldPassword, infoRequest.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                return CreateValidationErrors(changePasswordResult);
            }
        }

        if (!string.IsNullOrEmpty(infoRequest.NewEmail))
        {
            var email = await userManager.GetEmailAsync(user);

            if (email != infoRequest.NewEmail)
            {
                await SendConfirmationEmailAsync(user, userManager, context, infoRequest.NewEmail, isChange: true);
            }
        }

        return TypedResults.Ok(await CreateInfoResponseAsync(user, userManager));
    }

    private static async Task<Results<Ok, BadRequest<ValidationError>>> ResetPasswordAsync(
        [FromBody] ResetPasswordRequest resetRequest,
        [FromServices] IServiceProvider sp)
    {
        var userManager = sp.GetRequiredService<UserManager<User>>();

        var user = await userManager.FindByEmailAsync(resetRequest.Email);

        if (user is null || !(await userManager.IsEmailConfirmedAsync(user)))
        {
            // Don't reveal that the user does not exist or is not confirmed, so don't return a 200 if we would have
            // returned a 400 for an invalid code given a valid user email.
            return CreateValidationErrors(IdentityResult.Failed(userManager.ErrorDescriber.InvalidToken()));
        }

        IdentityResult result;
        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetRequest.ResetCode));
            result = await userManager.ResetPasswordAsync(user, code, resetRequest.NewPassword);
        }
        catch (FormatException)
        {
            result = IdentityResult.Failed(userManager.ErrorDescriber.InvalidToken());
        }

        if (!result.Succeeded)
        {
            return CreateValidationErrors(result);
        }

        return TypedResults.Ok();
    }

    private static async Task<InfoResponse> CreateInfoResponseAsync<TUser>(TUser user, UserManager<TUser> userManager)
        where TUser : class
    {
        return new()
        {
            Email = await userManager.GetEmailAsync(user) ??
                    throw new NotSupportedException("Users must have an email."),
            IsEmailVerified = await userManager.IsEmailConfirmedAsync(user),
        };
    }

    private static async Task<Results<Ok<TwoFactorResponse>, BadRequest<ValidationError>, NotFound>> Post2fa(
        ClaimsPrincipal claimsPrincipal,
        [FromBody] TwoFactorRequest tfaRequest,
        [FromServices] IServiceProvider sp)
    {
        var signInManager = sp.GetRequiredService<SignInManager<User>>();
        var userManager = signInManager.UserManager;
        if (await userManager.GetUserAsync(claimsPrincipal) is not { } user)
        {
            return TypedResults.NotFound();
        }

        if (tfaRequest.Enable == true)
        {
            var res = new ValidationError("Two-factor errors.");
            if (tfaRequest.ResetSharedKey)
            {
                res.Details["SharedKey"] =
                [
                    "Resetting the 2fa shared key must disable 2fa until a 2fa token based on the new shared key is validated."
                ];

                return TypedResults.BadRequest(res);
            }

            if (string.IsNullOrEmpty(tfaRequest.TwoFactorCode))
            {
                res.Details["TwoFactorCode"] =
                [
                    "No 2fa token was provided by the request. A valid 2fa token is required to enable 2fa."
                ];

                return TypedResults.BadRequest(res);
            }

            if (!await userManager.VerifyTwoFactorTokenAsync(
                    user,
                    userManager.Options.Tokens.AuthenticatorTokenProvider,
                    tfaRequest.TwoFactorCode))
            {
                res.Details["TwoFactorCode"] =
                [
                    "The 2fa token provided by the request was invalid. A valid 2fa token is required to enable 2fa."
                ];

                return TypedResults.BadRequest(res);
            }

            await userManager.SetTwoFactorEnabledAsync(user, true);
        }
        else if (tfaRequest.Enable == false || tfaRequest.ResetSharedKey)
        {
            await userManager.SetTwoFactorEnabledAsync(user, false);
        }

        if (tfaRequest.ResetSharedKey)
        {
            await userManager.ResetAuthenticatorKeyAsync(user);
        }

        string[]? recoveryCodes = null;
        if (tfaRequest.ResetRecoveryCodes ||
            (tfaRequest.Enable == true && await userManager.CountRecoveryCodesAsync(user) == 0))
        {
            var recoveryCodesEnumerable = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            recoveryCodes = recoveryCodesEnumerable?.ToArray();
        }

        if (tfaRequest.ForgetMachine)
        {
            await signInManager.ForgetTwoFactorClientAsync();
        }

        var key = await userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(key))
        {
            await userManager.ResetAuthenticatorKeyAsync(user);
            key = await userManager.GetAuthenticatorKeyAsync(user);

            if (string.IsNullOrEmpty(key))
            {
                throw new NotSupportedException("The user manager must produce an authenticator key after reset.");
            }
        }

        return TypedResults.Ok(new TwoFactorResponse
        {
            SharedKey = key,
            RecoveryCodes = recoveryCodes,
            RecoveryCodesLeft = recoveryCodes?.Length ?? await userManager.CountRecoveryCodesAsync(user),
            IsTwoFactorEnabled = await userManager.GetTwoFactorEnabledAsync(user),
            IsMachineRemembered = await signInManager.IsTwoFactorClientRememberedAsync(user),
        });
    }

    private static async Task<Results<ContentHttpResult, UnauthorizedHttpResult>> ConfirmEmailAsync(
        [FromQuery] string userId,
        [FromQuery] string code,
        [FromQuery] string? changedEmail,
        [FromServices] IServiceProvider sp)
    {
        var userManager = sp.GetRequiredService<UserManager<User>>();
        if (await userManager.FindByIdAsync(userId) is not { } user)
        {
            // We could respond with a 404 instead of a 401 like Identity UI, but that feels like unnecessary information.
            return TypedResults.Unauthorized();
        }

        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }
        catch (FormatException)
        {
            return TypedResults.Unauthorized();
        }

        IdentityResult result;

        if (string.IsNullOrEmpty(changedEmail))
        {
            result = await userManager.ConfirmEmailAsync(user, code);
        }
        else
        {
            // As with Identity UI, email and user name are one and the same. So when we update the email,
            // we need to update the user name.
            result = await userManager.ChangeEmailAsync(user, changedEmail, code);
            if (result.Succeeded)
            {
                result = await userManager.SetUserNameAsync(user, changedEmail);
            }
        }

        if (!result.Succeeded)
        {
            return TypedResults.Unauthorized();
        }

        return TypedResults.Text("Thank you for confirming your email.");
    }
}