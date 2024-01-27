using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Converters.Json
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private readonly TimezoneHandler timezoneHandler;

        public DateTimeConverter(TimezoneHandler timezoneHandler)
        {
            this.timezoneHandler = timezoneHandler;
        }
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(TimeZoneInfo.ConvertTimeFromUtc(value, TimeZoneInfo.FindSystemTimeZoneById(timezoneHandler.TimezoneId)).ToString());
        }
    }
}
