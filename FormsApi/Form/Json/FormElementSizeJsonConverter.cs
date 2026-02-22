using System.Text.Json;
using System.Text.Json.Serialization;
using FormsApi.Form.Primitives;

namespace FormsApi.Form.Json;

public class FormElementSizesonConverter : JsonConverter<FormElementSize>
{
    public const string AutoSize = "autosize";
    public override FormElementSize Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TryGetInt32(out int percent))
            return percent;
        if (reader.GetString() == AutoSize)
            return FormElementSize.AutoSize;
        return 100;
    }
    public override void Write(Utf8JsonWriter writer, FormElementSize value, JsonSerializerOptions options)
    {
        if (value is AutoSize)
            writer.WriteStringValue(AutoSize);
        else if (value is PercentSize pct)
            writer.WriteNumberValue(pct.Size);
    }
}
