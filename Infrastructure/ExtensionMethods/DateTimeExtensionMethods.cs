namespace Infrastructure.ExtensionMethods;

public static class DateTimeExtensionMethods
{
    public static DateTime ConvertUTCToLocalTime(this DateTime utcDateTime, TimezoneHandler timezoneHandler)
        => TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, TimeZoneInfo.FindSystemTimeZoneById(timezoneHandler.TimezoneId));
    public static DateTime? ConvertUTCToLocalTime(this DateTime? utcDateTime, TimezoneHandler timezoneHandler)
        => utcDateTime.HasValue ? TimeZoneInfo.ConvertTimeFromUtc(utcDateTime.Value, TimeZoneInfo.FindSystemTimeZoneById(timezoneHandler.TimezoneId)) : utcDateTime;
}
