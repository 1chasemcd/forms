using System.Text.Json;

namespace FormsApi.Recalculate;

public sealed class RecalculateEventResult
{
    public JsonElement? Model { get; init; }
    public PostRecalculateEvent? PostEvent { get; init; }
}
