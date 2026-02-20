namespace FormsApi.Form.View;

public sealed record class SubpropertyGridView : GridView
{
    public required string SubPropertyName { get; init; }
}
