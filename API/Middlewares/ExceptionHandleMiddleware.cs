using Contracts.Shared;

namespace API.Middlewares;
public class ExceptionHandleMiddleware
{
    private readonly RequestDelegate _next;
    private ILogger<ExceptionHandleMiddleware>? _logger { get; set; }
    public ExceptionHandleMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, ILogger<ExceptionHandleMiddleware> logger)
    {
        _logger = logger;
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleException(ex, httpContext);
        }
    }

    private async Task HandleException(Exception ex, HttpContext httpContext)
    {
        var innerEx = !string.IsNullOrEmpty(ex.InnerException?.Message) ? $"INNER EX. MSG: {ex.InnerException?.Message}" : string.Empty;
        if (string.IsNullOrEmpty(innerEx))
            _logger?.LogError($@"---------------------------------
                                    {httpContext.Request.Path.Value}
                                    REASON: {ex.Message.Replace(Environment.NewLine, string.Empty)}
                                    Stack Trace: {ex.StackTrace}
                                    ---------------------------------");
        else
            _logger?.LogError($@"---------------------------------
                                    PATH: {httpContext.Request.Path.Value}
                                    REASON: {ex.Message.Replace(Environment.NewLine, string.Empty)}
                                    INNER EXCEPTION:{innerEx}
                                    STACK TRACE: {ex.StackTrace}
                                    ---------------------------------");

        if (ex is BusinessException)
        {
            httpContext.Response.StatusCode = 400;

            await httpContext.Response.WriteAsJsonAsync(new
            {
                ex.Message,
                ((BusinessException)ex).Errors
            });
        }
        else
        {
            httpContext.Response.StatusCode = 500;

            await httpContext.Response.WriteAsJsonAsync(new
            {
                Message = "Unknown Error"
            });
        }
    }
}

public static class ExceptionHandleMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandleMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandleMiddleware>();
    }
}
