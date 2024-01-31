using Microsoft.AspNetCore.Http.Features;
using Serilog.AspNetCore;
using Serilog.Events;
using Serilog.Extensions.Hosting;
using Serilog.Parsing;
using Serilog;
using System.Diagnostics;
using ILogger = Serilog.ILogger;
using Microsoft.Extensions.Options;

namespace API.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    private readonly DiagnosticContext _diagnosticContext;

    private readonly MessageTemplate _messageTemplate;

    private readonly Action<IDiagnosticContext, HttpContext>? _enrichDiagnosticContext;

    private readonly Func<HttpContext, double, Exception?, LogEventLevel> _getLevel;

    private readonly Func<HttpContext, string, double, int, IEnumerable<LogEventProperty>> _getMessageTemplateProperties;

    private readonly ILogger? _logger;

    private readonly bool _includeQueryInRequestPath;

    private static readonly LogEventProperty[] NoProperties = new LogEventProperty[0];

    public RequestLoggingMiddleware(RequestDelegate next, DiagnosticContext diagnosticContext, RequestLoggingOptions options)
    {
        if (options == null)
        {
            throw new ArgumentNullException("options");
        }

        _next = next ?? throw new ArgumentNullException("next");
        _diagnosticContext = diagnosticContext ?? throw new ArgumentNullException("diagnosticContext");
        _getLevel = options.GetLevel;
        _enrichDiagnosticContext = options.EnrichDiagnosticContext;
        _messageTemplate = new MessageTemplateParser().Parse(options.MessageTemplate);
        _logger = options.Logger?.ForContext<RequestLoggingMiddleware>();
        _includeQueryInRequestPath = options.IncludeQueryInRequestPath;
        _getMessageTemplateProperties = options.GetMessageTemplateProperties;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        if (httpContext == null)
        {
            throw new ArgumentNullException("httpContext");
        }

        long start = Stopwatch.GetTimestamp();
        DiagnosticContextCollector collector = _diagnosticContext.BeginCollection();
        try
        {
            await _next(httpContext);
            double elapsedMilliseconds = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());
            int statusCode = httpContext.Response.StatusCode;
            LogCompletion(httpContext, collector, statusCode, elapsedMilliseconds, null);
        }
        catch (Exception ex) when (LogCompletion(httpContext, collector, 500, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()), ex))
        {
        }
        finally
        {
            collector.Dispose();
        }
    }

    private bool LogCompletion(HttpContext httpContext, DiagnosticContextCollector collector, int statusCode, double elapsedMs, Exception? ex)
    {
        ILogger logger = _logger ?? Log.ForContext<RequestLoggingMiddleware>();
        LogEventLevel level = _getLevel(httpContext, elapsedMs, ex);
        if (!logger.IsEnabled(level))
        {
            return false;
        }

        _enrichDiagnosticContext?.Invoke(_diagnosticContext, httpContext);
        if (!collector.TryComplete(out var properties, out var exception))
        {
            properties = NoProperties;
        }

        IEnumerable<LogEventProperty> properties2 = properties.Concat(_getMessageTemplateProperties(httpContext, GetPath(httpContext, _includeQueryInRequestPath), elapsedMs, statusCode));
        LogEvent logEvent = new LogEvent(DateTimeOffset.Now, level, ex ?? exception, _messageTemplate, properties2);
        logger.Write(logEvent);
        return false;
    }

    private static double GetElapsedMilliseconds(long start, long stop)
    {
        return (double)((stop - start) * 1000) / (double)Stopwatch.Frequency;
    }

    private static string GetPath(HttpContext httpContext, bool includeQueryInRequestPath)
    {
        string text = ((!includeQueryInRequestPath) ? httpContext.Features.Get<IHttpRequestFeature>()?.Path : httpContext.Features.Get<IHttpRequestFeature>()?.RawTarget);
        if (string.IsNullOrEmpty(text))
        {
            text = httpContext.Request.Path.ToString();
        }

        return text;
    }
}
public static class SerilogApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSerilogRequestLogging(this IApplicationBuilder app, Action<RequestLoggingOptions>? configureOptions = null)
    {
        if (app == null)
        {
            throw new ArgumentNullException("app");
        }

        RequestLoggingOptions requestLoggingOptions = app.ApplicationServices.GetService<IOptions<RequestLoggingOptions>>()?.Value ?? new RequestLoggingOptions();
        configureOptions?.Invoke(requestLoggingOptions);
        if (requestLoggingOptions.MessageTemplate == null)
        {
            throw new ArgumentException("MessageTemplate cannot be null.");
        }

        if (requestLoggingOptions.GetLevel == null)
        {
            throw new ArgumentException("GetLevel cannot be null.");
        }

        return app.UseMiddleware<RequestLoggingMiddleware>(new object[1] { requestLoggingOptions });
    }
}