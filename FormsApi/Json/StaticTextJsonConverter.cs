using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using FormsApi.Common.Types;

namespace FormsApi.Json;

internal sealed class StaticTextJsonConverter : JsonConverter<StaticText>
{
    public override StaticText Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString() ?? "";
    }
    public override void Write(Utf8JsonWriter writer, StaticText value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}
