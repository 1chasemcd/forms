using System.ComponentModel.DataAnnotations;

namespace FormsApi.Form.Primitives;

public sealed record FormAction
{
    [Required]
    public required SerializedType Service { get; init; }
    [Required]
    public required string Method { get; init; }
}
