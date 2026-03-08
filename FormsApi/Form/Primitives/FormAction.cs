namespace FormsApi.Form.Primitives;

public sealed record FormAction
{
    public required SerializedType Service { get; init; }
    public required string Method { get; init; }
}
