using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FormsApi.Form.Primitives;

public sealed class RecalculateEvent
{
    [Required]
    public required SerializedType Service { get; init; }
    [Required]
    public required string Method { get; init; }
    [Required]
    public required PropertiesToSendCollection PropertiesToSend { get; init; }
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(SendAll), "sendall")]
[JsonDerivedType(typeof(SendNone), "sendnone")]
[JsonDerivedType(typeof(SendSome), "sendsome")]

public abstract record PropertiesToSendCollection;
public sealed record SendAll : PropertiesToSendCollection;
public sealed record SendNone : PropertiesToSendCollection;
public sealed record SendSome : PropertiesToSendCollection
{
    [Required]
    public required IEnumerable<string> Names { get; init; }
}
