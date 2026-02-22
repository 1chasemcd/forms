using System.Text.Json;
using System.Text.Json.Serialization;
using FormsApi.Form.Primitives;

namespace FormsApi.Form.Json;

public class RepositoryTypeJsonConverter : JsonConverter<RepositoryType>
{
    public override RepositoryType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        _ = RepositoryType.TryParse(reader.GetString(), out RepositoryType result);
        return result;
    }
    public override void Write(Utf8JsonWriter writer, RepositoryType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
