using System;
using System.Buffers.Text;
using System.Buffers;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure.Converters.Json
{
    public class IDictionaryJsonConvert : JsonConverter<IDictionary<string, object>>
    {
        public override IDictionary<string, object> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var dictionary = new Dictionary<string, object>();

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

                string propertyName = reader.GetString();
                reader.Read();

                object value = null;

                switch (reader.TokenType)
                {
                    case JsonTokenType.Null:
                        value = null;
                        break;
                    case JsonTokenType.True:
                        value = true;
                        break;
                    case JsonTokenType.False:
                        value = false;
                        break;
                    case JsonTokenType.Number:
                        // This handles both integer and floating-point numbers
                        value = reader.TryGetInt64(out long l) ? l : reader.GetDouble();
                        break;
                    case JsonTokenType.String:
                        value = reader.GetString();
                        break;
                    case JsonTokenType.StartObject:
                        value = Read(ref reader, typeToConvert, options);
                        break;
                    case JsonTokenType.StartArray:
                        value = JsonSerializer.Deserialize<List<object>>(ref reader, options);
                        break;
                    default:
                        throw new JsonException();
                }

                dictionary[propertyName] = value;
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, IDictionary<string, object> value, JsonSerializerOptions options)
        {
            // Writing is straightforward with JsonSerializer
            JsonSerializer.Serialize(writer, value, options);
        }
    }

}
