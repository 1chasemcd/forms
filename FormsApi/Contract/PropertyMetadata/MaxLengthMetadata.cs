using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.PropertyMetadata;

public sealed record MaxLengthMetadata : PropertyMetadata
{
    [Required]
    public required FormValueRef Value { get; init; }
}
