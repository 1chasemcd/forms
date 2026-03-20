using System.Text.Json.Serialization;
using FormsApi.Json;

namespace FormsApi.Common.Types;

[JsonConverter(typeof(TextAreaJsonConverter))]
public sealed record TextArea
{
    private readonly string _text;
    public static implicit operator string(TextArea text) => text._text;
    public static implicit operator TextArea(string text) => new(text);
    private TextArea(string text) => _text = text;
}
