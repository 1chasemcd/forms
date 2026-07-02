using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.PropertyMetadata;

public sealed record MaxValueMetadata : PropertyMetadata
{
    [Required]
    public required FormValueRef Value { get; init; }
}
