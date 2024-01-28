using System.Globalization;

namespace API.Middlewares;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;
    private ILogger<CultureMiddleware>? _logger { get; set; }
    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, ILogger<CultureMiddleware> logger)
    {
        _logger = logger;

        if (httpContext.Request.Headers.ContainsKey("culture"))
        {
            _logger.LogDebug(httpContext.Request.Headers["culture"]);
            CultureInfo.CurrentCulture = new(httpContext.Request.Headers["culture"]!);
            CultureInfo.CurrentUICulture = new(httpContext.Request.Headers["culture"]!);
        }
        await _next(httpContext);
    }
}
public static class CultureMiddlewareExtensions
{
    public static IApplicationBuilder UseCultureMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CultureMiddleware>();
    }
}
