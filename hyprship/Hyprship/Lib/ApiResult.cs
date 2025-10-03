using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;

namespace Hyprship.Lib;

public class ApiResult
{
    public bool IsOk { get; set; }

    public Error? Error { get; set; }
}

public class ApiResult<TValue> : ApiResult
{
    public TValue? Value { get; set; }

    public static implicit operator ApiResult<TValue>(TValue value)
        => new() { Value = value, IsOk = true };

    public static implicit operator ApiResult<TValue>(Error error)
        => new() { Error = error, IsOk = false };

    public Ok<ApiResult<TValue>> Ok(TValue value)
    {
        this.Value = value;
        this.IsOk = true;
        return TypedResults.Ok(this);
    }

    public BadRequest<ApiResult<TValue>> BadRequest(Error error)
    {
        this.Error = error;
        this.IsOk = false;
        return TypedResults.BadRequest(this);
    }

    public NotFound<ApiResult<TValue>> NotFound(Error error)
    {
        this.Error = error;
        this.IsOk = false;
        return TypedResults.NotFound(this);
    }

    public InternalServerError<ApiResult<TValue>> ServerError(Error error)
    {
        this.Error = error;
        this.IsOk = false;
        return TypedResults.InternalServerError(this);
    }
}

public class ApiResult<T, TValue> : ApiResult<TValue>
    where T : ApiResult<T, TValue>, new()
{
    public TValue? Value { get; set; }

    public new Ok<T> Ok(TValue value)
    {
        this.Value = value;
        this.IsOk = true;
        return TypedResults.Ok((T)this);
    }

    public new BadRequest<T> BadRequest(Error error)
    {
        this.Error = error;
        this.IsOk = false;
        return TypedResults.BadRequest((T)this);
    }

    public new NotFound<T> NotFound(Error error)
    {
        this.Error = error;
        this.IsOk = false;
        return TypedResults.NotFound((T)this);
    }

    public new InternalServerError<T> ServerError(Error error)
    {
        this.Error = error;
        this.IsOk = false;
        return TypedResults.InternalServerError((T)this);
    }

    public JsonHttpResult<T> Fail(Error error, int statusCode = StatusCodes.Status400BadRequest)
    {
        this.Error = error;
        this.IsOk = false;
        return TypedResults.Json((T)this, (JsonSerializerOptions?)null, "application/json", statusCode);
    }
}

/*
public sealed class Failure<TValue> : IResult, IEndpointMetadataProvider, IStatusCodeHttpResult, IValueHttpResult,
    IValueHttpResult<TValue>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequest"/> class with the values
    /// provided.
    /// </summary>
    /// <param name="error">The error content to format in the entity body.</param>
    internal Failure(TValue? error)
    {
        this.Value = error;
    }

    /// <summary>
    /// Gets the object result.
    /// </summary>
    public TValue? Value { get; }

    object? IValueHttpResult.Value => this.Value;

    /// <summary>
    /// Gets the HTTP status code: <see cref="StatusCodes.Status400BadRequest"/>
    /// </summary>
    public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;

    int? IStatusCodeHttpResult.StatusCode => this.StatusCode;

    /// <inheritdoc/>
    public Task ExecuteAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        // Creating the logger with a string to preserve the category after the refactoring.
        var loggerFactory = httpContext.RequestServices.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("Microsoft.AspNetCore.Http.Result.FailureObjectResult");
        httpContext.Response.StatusCode = this.StatusCode;

        return WriteResultAsJsonAsync(
            httpContext,
            logger: logger,
            this.Value);
    }

    /// <inheritdoc/>
    static void IEndpointMetadataProvider.PopulateMetadata(MethodInfo method, EndpointBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(method);
        ArgumentNullException.ThrowIfNull(builder);

        builder.Metadata.Add(new ProducesResponseTypeMetadata(
            , typeof(TValue), ContentTypeConstants.ApplicationJsonContentTypes));
        builder.Metadata.Add(DisableCookieRedirectMetadata.Instance);
    }

    public static Task WriteResultAsJsonAsync<TValue>(
            HttpContext httpContext,
            ILogger logger,
            TValue? value,
            string? contentType = null,
            JsonSerializerOptions? jsonSerializerOptions = null)
        {
            if (value is null)
            {
                return Task.CompletedTask;
            }

           jsonSerializerOptions ??= ResolveJsonOptions(httpContext).SerializerOptions;
            var jsonTypeInfo = (JsonTypeInfo<TValue>)jsonSerializerOptions.GetTypeInfo(typeof(TValue));

            Type? runtimeType = value.GetType();
            if (jsonTypeInfo.ShouldUseWith(runtimeType))
            {
                Log.WritingResultAsJson(logger, jsonTypeInfo.Type.Name);
                return httpContext.Response.WriteAsJsonAsync(
                    value,
                    jsonTypeInfo,
                    contentType: contentType);
            }

            Log.WritingResultAsJson(logger, runtimeType.Name);
            // Since we don't know the type's polymorphic characteristics
            // our best option is to serialize the value as 'object'.
            // call WriteAsJsonAsync<object>() rather than the declared type
            // and avoid source generators issues.
            // https://github.com/dotnet/aspnetcore/issues/43894
            // https://learn.microsoft.com/dotnet/standard/serialization/system-text-json-polymorphism
            return httpContext.Response.WriteAsJsonAsync<object>(
                value,
                jsonSerializerOptions,
                contentType: contentType);
        }

}
*/

public class Error
{
    public Error()
    {
        this.TraceId = Activity.Current?.TraceId.ToString();
        this.ParentId = Activity.Current?.ParentId;
    }

    public Error(string message)
    {
        this.Message = message;
        this.ParentId = Activity.Current?.ParentId;
        this.TraceId = Activity.Current?.TraceId.ToString();
    }

    public Error(string message, string code)
    {
        this.Message = message;
        this.Code = code;
        this.ParentId = Activity.Current?.ParentId;
        this.TraceId = Activity.Current?.TraceId.ToString();
    }

    public Error(string message, string code, OrderedDictionary<string, object> details)
    {
        this.Message = message;
        this.Code = code;
        this.ParentId = Activity.Current?.ParentId;
        this.TraceId = Activity.Current?.TraceId.ToString();
    }

    public Error(string message, string code, IEnumerable<Error> errors)
    {
        this.Message = message;
        this.Code = code;
        this.ParentId = Activity.Current?.ParentId;
        this.TraceId = Activity.Current?.TraceId.ToString();
        this.Errors = errors.ToList();
    }

    public static Func<Exception, Error> ConvertException { get; set; } = (ex) =>
    {
        return new(ex.Message);
    };

    public static Error Convert(Exception ex)
        => ConvertException.Invoke(ex);

    public string Message { get; set; } = string.Empty;

    public string Code { get; set; } = "Error";

    public OrderedDictionary<string, object?> Details { get; set; } = new();

    public List<Error> Errors { get; set; } = new();

    public string? Help { get; set; }

    public string? TraceId { get; set; }

    public string? ParentId { get; set; }
}