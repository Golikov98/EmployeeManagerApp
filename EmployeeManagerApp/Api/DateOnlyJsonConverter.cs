using System.Text.Json;
using System.Text.Json.Serialization;

namespace EmployeeManagerApp.Api
{
    internal class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private const string Format = "yyyy-MM-dd";

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var str = reader.GetString();
                if (DateOnly.TryParse(str, out var date))
                    return date;

                if (DateTime.TryParse(str, out var dt))
                    return DateOnly.FromDateTime(dt);
            }
            throw new JsonException($"Cannot convert {reader.GetString()} to DateOnly");
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }
}
