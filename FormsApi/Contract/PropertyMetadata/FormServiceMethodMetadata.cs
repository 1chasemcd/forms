using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.PropertyMetadata;

public sealed record FormServiceMethodMetadata : PropertyMetadata
{
    [Required]
    public required FormServiceMethod Value { get; init; }
}
