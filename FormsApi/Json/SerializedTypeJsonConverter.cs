using System.Text.Json;
using System.Text.Json.Serialization;
using FormsApi.Contract;

namespace FormsApi.Json;

internal sealed class SerializedTypeJsonConverter : JsonConverter<TypeDto>
{
    public override TypeDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        _ = TypeDto.TryParse(reader.GetString(), out TypeDto result);
        return result;
    }
    public override void Write(Utf8JsonWriter writer, TypeDto value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
