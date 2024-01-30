using Infrastructure.Converters.Json;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace API.Middlewares;

public class RequestLoggerMiddleware
{
    private readonly RequestDelegate _next;
    private ILogger<RequestLoggerMiddleware> _logger { get; set; } = default!;

    public RequestLoggerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, ILogger<RequestLoggerMiddleware> logger)
    {
        _logger = logger;

        await LogRequest(httpContext);

        await _next(httpContext);
    }

    private async Task LogRequest(HttpContext context)
    {
        _logger.LogInformation("{protocol} {method} {path}", context.Request.Protocol, context.Request.Method.ToUpper(), context.Request.Path);

        context.Request.EnableBuffering();

        var requestReader = new StreamReader(context.Request.Body);

        var content = await requestReader.ReadToEndAsync();

        var options = new JsonSerializerOptions();

        options.Converters.Add(new IDictionaryJsonConvert());

        if (!string.IsNullOrEmpty(content))
            _logger.LogInformation("Body {body}", JsonSerializer.Deserialize<IDictionary<string, object>>(content, options));

        context.Request.Body.Position = 0;
    }
}

public static class RequestLoggerMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLoggerMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLoggerMiddleware>();
    }
}
