using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.PropertyMetadata;

public sealed record ServiceMethodMetadata : PropertyMetadata
{
    [Required]
    public required ServiceMethod Value { get; init; }
}
