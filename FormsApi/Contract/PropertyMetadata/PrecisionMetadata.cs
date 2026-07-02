using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.PropertyMetadata;

public sealed record PrecisionMetadata : PropertyMetadata
{
    [Required]
    public required FormValueRef Value { get; init; }
}
