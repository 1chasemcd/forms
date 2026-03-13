using System.Text.Json.Serialization;
using FormsApi.Json;

namespace FormsApi.Common.Types;

[JsonConverter(typeof(StaticTextJsonConverter))]
public sealed record StaticText
{
    private readonly string _text;
    public static implicit operator string(StaticText text) => text._text;
    public static implicit operator StaticText(string text) => new(text);
    private StaticText(string text) => _text = text;
}
