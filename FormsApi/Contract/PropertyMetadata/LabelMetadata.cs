using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.PropertyMetadata;

public sealed record LabelMetadata : PropertyMetadata
{
    [Required]
    public required FormValueRef Value { get; init; }
}
