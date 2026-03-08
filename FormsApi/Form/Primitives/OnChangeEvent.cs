namespace FormsApi.Form.Primitives;

public sealed class OnChangeEvent
{
    public FormAction? FormAction { get; init; }
    public IEnumerable<string>? PropertiesToUpdate { get; init; }
}
