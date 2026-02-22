using System.Text.Json;
using System.Text.Json.Serialization;
using FormsApi.Form.Primitives;

namespace FormsApi.Form.Json;

public class PropertyOrConstantJsonConverter : JsonConverter<PropertyOrConstant>
{
    public const string Property = "property";
    public const string Constant = "constant";
    public override PropertyOrConstant Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // string → Property
        if (reader.TokenType == JsonTokenType.String)
            return new Property(reader.GetString()!);

        // anything else → Constant
        // var value = JsonSerializer.Deserialize(ref reader, options);
        return new Constant("");
    }

    public override void Write(Utf8JsonWriter writer, PropertyOrConstant value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case Property p:
                writer.WriteString(Property, p.Value);
                break;

            case Constant c:
                JsonSerializer.Serialize(writer, c.Value, options);
                break;
        }
    }
}
