using Infrastructure.Converters.Json;
using System.Diagnostics;
using System.Text.Json;

namespace API.Middlewares;

public class RequestLoggerMiddleware
{
    private readonly RequestDelegate _next;
    private ILogger<RequestLoggerMiddleware> _logger { get; set; } = default!;
    private long _timestamp { get; set; }
    public RequestLoggerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, ILogger<RequestLoggerMiddleware> logger)
    {
        _logger = logger;
        try
        {
            _timestamp = Stopwatch.GetTimestamp();
            await _next(httpContext);
            await LogRequest(httpContext);
        }
        catch
        {
            await LogRequest(httpContext);
            throw;
        }
    }

    private async Task LogRequest(HttpContext context)
    {
        context.Request.EnableBuffering();

        var requestReader = new StreamReader(context.Request.Body);

        var content = await requestReader.ReadToEndAsync();

        var options = new JsonSerializerOptions();

        options.Converters.Add(new IDictionaryJsonConvert());

        double elapsedMilliseconds = GetElapsedMilliseconds(_timestamp, Stopwatch.GetTimestamp());

        _logger.LogInformation("{method} {path} {status} {elapsed} ms", context.Request.Method.ToUpper(), context.Request.Path, context.Response.StatusCode, elapsedMilliseconds);

        if (!string.IsNullOrEmpty(content))
            _logger.LogInformation("Body {body}", JsonSerializer.Deserialize<IDictionary<string, object>>(content, options));

        context.Request.Body.Position = 0;
    }

    private static double GetElapsedMilliseconds(long start, long stop)
        => (double)((stop - start) * 1000) / (double)Stopwatch.Frequency;
}

public static class RequestLoggerMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLoggerMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLoggerMiddleware>();
    }
}
