namespace FormsApi.Form.Primitives;

public sealed class RecalculateEvent
{
    public FormAction? FormAction { get; init; }
    public IEnumerable<string>? PropertiesToUpdate { get; init; }
}
