namespace Hyprship.Lib;

public static partial class Log
{
    [LoggerMessage(1, LogLevel.Information,
        "Setting HTTP status code {StatusCode}.",
        EventName = "WritingResultAsStatusCode")]
    public static partial void WritingResultAsStatusCode(ILogger logger, int statusCode);
    
    [LoggerMessage(3, LogLevel.Information, "Writing value of type '{Type}' as Json.",
        EventName = "WritingResultAsJson")]
    public static partial void WritingResultAsJson(ILogger logger, string type);
}