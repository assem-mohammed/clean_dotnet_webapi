using Infrastructure;
using Infrastructure.Converters.Json;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace API.Middlewares
{
    public class TimeZoneMiddleware
    {
        private readonly RequestDelegate _next;

        private ILogger<TimeZoneMiddleware> _logger { get; set; } = default!;
        public TimeZoneMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, ILogger<TimeZoneMiddleware> logger, TimezoneHandler timezoneHandler)
        {
            _logger = logger;

            if (httpContext.Request.Headers.ContainsKey("time-zone"))
            {
                _logger.LogDebug(httpContext.Request.Headers["time-zone"]);
                timezoneHandler.TimezoneId = httpContext.Request.Headers["time-zone"]!;
            }
            else
                timezoneHandler.TimezoneId = TimeZoneInfo.Local.StandardName;

            await _next(httpContext);
        }
    }
    public static class TimeZoneMiddlewareExtensions
    {
        public static IApplicationBuilder UseTimeZoneMiddleware(this IApplicationBuilder builder)
            => builder.UseMiddleware<TimeZoneMiddleware>();
    }
}
