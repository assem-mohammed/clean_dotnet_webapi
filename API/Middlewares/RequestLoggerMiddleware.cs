using System.Text;

namespace API.Middlewares;

public class RequestLoggerMiddleware
{
    private readonly RequestDelegate _next;
    private ILogger<CultureMiddleware> _logger { get; set; } = default!;

    public RequestLoggerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, ILogger<CultureMiddleware> logger)
    {
        _logger = logger;

        await LogRequest(httpContext);

        await _next(httpContext);
    }

    private async Task LogRequest(HttpContext context)
    {
        var requestContent = new StringBuilder().AppendLine();

        requestContent.AppendLine("=== Request Info ===");

        requestContent.AppendLine($"METHOD = {context.Request.Method.ToUpper()}");

        requestContent.AppendLine($"PATH = {context.Request.Path}");

        context.Request.EnableBuffering();

        var requestReader = new StreamReader(context.Request.Body);

        var content = await requestReader.ReadToEndAsync();

        if (!string.IsNullOrEmpty(content))
        {
            requestContent.AppendLine("-- BODY");

            requestContent.AppendLine($"{content}");
        }

        _logger.LogInformation(requestContent.ToString());

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
