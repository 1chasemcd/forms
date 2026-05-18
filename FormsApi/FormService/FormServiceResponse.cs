using System.Text.Json;

namespace FormsApi.FormService;

public sealed class FormServiceResponse
{
    public JsonElement? Model { get; init; }
    public FormServicePostAction? PostAction { get; init; }
}
