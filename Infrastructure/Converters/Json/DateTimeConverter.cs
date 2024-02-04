using System.Text.Json.Serialization;
using System.Text.Json;

namespace Infrastructure.Converters.Json;

public class DateTimeConverter : JsonConverter<DateTime>
{
    private readonly TimezoneHandler? timezoneHandler;
    public DateTimeConverter(TimezoneHandler? timezoneHandler = null)
    {
        this.timezoneHandler = timezoneHandler;
    }
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.Parse(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        //writer.WriteStringValue(value.ToString("dd-MM-yyyy HH:mm"));
        if (timezoneHandler != null)
            writer.WriteStringValue(TimeZoneInfo.ConvertTimeFromUtc(value, TimeZoneInfo.FindSystemTimeZoneById(timezoneHandler.TimezoneId)).ToString());
        else
            writer.WriteStringValue(value.ToString());
    }
}
