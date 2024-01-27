using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExtensionMethods
{
    public static class DateTimeExtensionMethods
    {
        public static DateTime ConvertUTCToLocalTime(this DateTime utcDateTime, TimezoneHandler timezoneHandler)
            => TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, TimeZoneInfo.FindSystemTimeZoneById(timezoneHandler.TimezoneId));
    }
}
