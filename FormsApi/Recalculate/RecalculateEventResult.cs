namespace FormsApi.Recalculate;

public sealed class RecalculateEventResult
{
    public object? Model { get; init; }
    public PostRecalculateEvent? PostEvent { get; init; }
}
