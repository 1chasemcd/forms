using System.Text.Json;
using FormsApi.Contract.PostRequest;

namespace FormsApi.ServiceMethod;

public sealed record ServiceMethodResponse
{
    public JsonElement? Model { get; init; }
    public PostRequestAction? PostAction { get; init; }
}
