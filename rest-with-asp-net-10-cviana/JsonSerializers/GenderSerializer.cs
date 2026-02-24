using System.Text.Json;
using System.Text.Json.Serialization;

namespace rest_with_asp_net_10_cviana.JsonSerializers
{
    public class GenderSerializer : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
         => reader.GetString();

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            var formated = value.StartsWith("m", StringComparison.CurrentCultureIgnoreCase) ? "masculino" : "feminino";
            writer.WriteStringValue(formated);
        }
    }
}
