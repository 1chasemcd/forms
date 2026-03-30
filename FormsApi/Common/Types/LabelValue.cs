using System.Text.Json.Serialization;
using FormsApi.Json;

namespace FormsApi.Common.Types;

[JsonConverter(typeof(LabelValueJsonConverter))]
public sealed record LabelValue
{
    private readonly string _text;
    public static implicit operator string(LabelValue text) => text._text;
    public static implicit operator LabelValue(string text) => new(text);
    private LabelValue(string text) => _text = text;
}
