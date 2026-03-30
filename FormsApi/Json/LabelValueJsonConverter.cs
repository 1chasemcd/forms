using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using FormsApi.Common.Types;

namespace FormsApi.Json;

internal sealed class LabelValueJsonConverter : JsonConverter<LabelValue>
{
    public override LabelValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString() ?? "";
    }
    public override void Write(Utf8JsonWriter writer, LabelValue value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}
