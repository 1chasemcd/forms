using System.Text.Json;
using System.Text.Json.Serialization;
using FormsApi.Definition.Primitives;

namespace FormsApi.Json;

internal sealed class FormElementSizeJsonConverter : JsonConverter<FormElementSize>
{
    public const int AutoSize = -1;
    public override FormElementSize Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TryGetInt32(out int value))
            return value == AutoSize ? FormElementSize.AutoSize : value;
        return 0;
    }
    public override void Write(Utf8JsonWriter writer, FormElementSize value, JsonSerializerOptions options)
    {
        if (value is AutoSize)
            writer.WriteNumberValue(AutoSize);
        else if (value is NumericSize pct)
            writer.WriteNumberValue(pct.Size);
    }
}
