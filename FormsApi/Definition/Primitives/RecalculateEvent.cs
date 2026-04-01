using System.ComponentModel.DataAnnotations;

namespace FormsApi.Definition.Primitives;

public sealed class RecalculateEvent
{
    [Required]
    public required SerializedType Service { get; init; }
    [Required]
    public required string Method { get; init; }
}
