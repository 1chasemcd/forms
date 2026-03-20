using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using FormsApi.Common.Types;

namespace FormsApi.Json;

public class TextAreaJsonConverter : JsonConverter<TextArea>
{
    public override TextArea Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString() ?? "";
    }
    public override void Write(Utf8JsonWriter writer, TextArea value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}
