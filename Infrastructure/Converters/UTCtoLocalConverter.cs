using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Converters
{
    public class UTCtoLocalConverter : ValueConverter<DateTime, DateTime>
    {
        public UTCtoLocalConverter(TimezoneHandler timezoneHandler) : base(
            // writing to the database
            utc => utc,
           // reading from the database
           utc => TimeZoneInfo.ConvertTimeFromUtc(utc, TimeZoneInfo.FindSystemTimeZoneById(timezoneHandler.TimezoneId)))
        {
        }
    }

}
