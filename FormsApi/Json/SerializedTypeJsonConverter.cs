using System.Text.Json;
using System.Text.Json.Serialization;
using FormsApi.Form.Primitives;

namespace FormsApi.Json;

internal sealed class SerializedTypeJsonConverter : JsonConverter<SerializedType>
{
    public override SerializedType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        _ = SerializedType.TryParse(reader.GetString(), out SerializedType result);
        return result;
    }
    public override void Write(Utf8JsonWriter writer, SerializedType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
