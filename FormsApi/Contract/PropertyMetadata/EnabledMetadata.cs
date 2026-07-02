using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.PropertyMetadata;

public sealed record EnabledMetadata : PropertyMetadata
{
    [Required]
    public required FormValueRef Value { get; init; }
}
