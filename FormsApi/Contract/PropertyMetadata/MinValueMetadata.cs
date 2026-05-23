using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.PropertyMetadata;

public sealed record MinValueMetadata : PropertyMetadata
{
    [Required]
    public required PropertyOrConstant Value { get; init; }
}
