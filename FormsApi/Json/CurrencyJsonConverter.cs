using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using FormsApi.Common.Types;

namespace FormsApi.Json;

internal sealed class CurrencyJsonConverter : JsonConverter<Currency>
{
    public override Currency Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetDecimal();
    }
    public override void Write(Utf8JsonWriter writer, Currency value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}
