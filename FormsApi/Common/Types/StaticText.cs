namespace FormsApi.Common.Types;

public sealed record StaticText
{
    private readonly string _text;
    public static implicit operator string(StaticText text) => text._text;
    public static implicit operator StaticText(string text) => new(text);
    private StaticText(string text) => _text = text;
}
