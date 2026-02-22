namespace FormsApi.Form.View;

public sealed record class SubPropertyGridView : GridView
{
    public required string SubPropertyName { get; init; }
}
