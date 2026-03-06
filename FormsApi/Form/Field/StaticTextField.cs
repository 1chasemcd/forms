namespace FormsApi.Form.Field;

public sealed record class StaticTextField : BaseField
{
    public override string Id => Guid.NewGuid().ToString();
    public required string Text { get; init; }
}
