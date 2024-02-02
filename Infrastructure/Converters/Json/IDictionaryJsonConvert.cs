using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Converters.Json
{
    public class IDictionaryJsonConvert : JsonConverter<IDictionary<string, object?>>
    {
        public override IDictionary<string, object?> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var dictionary = new Dictionary<string, object?>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return dictionary;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                string propertyName = reader.GetString()!;

                reader.Read();
                object? value = reader.TokenType switch
                {
                    JsonTokenType.Null => null,
                    JsonTokenType.True => true,
                    JsonTokenType.False => false,
                    JsonTokenType.Number => reader.TryGetInt64(out long l) ? l : reader.GetDouble(),// This handles both integer and floating-point numbers
                    JsonTokenType.String => reader.GetString(),
                    JsonTokenType.StartObject => Read(ref reader, typeToConvert, options),
                    JsonTokenType.StartArray => JsonSerializer.Deserialize<List<object>>(ref reader, options),
                    _ => throw new JsonException(),
                };
                dictionary[propertyName] = value;
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, IDictionary<string, object?> value, JsonSerializerOptions options)
        {
            // Writing is straightforward with JsonSerializer
            JsonSerializer.Serialize(writer, value, options);
        }
    }

}
