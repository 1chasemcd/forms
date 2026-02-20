namespace FormsApi.Form.Field;

public sealed record class StaticTextField : BaseField
{
    public required string Text { get; init; }
}
