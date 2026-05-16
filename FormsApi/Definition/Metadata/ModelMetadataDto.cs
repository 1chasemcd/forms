using System.ComponentModel.DataAnnotations;
using FormsApi.Definition.Primitives;

namespace FormsApi.Definition.Metadata;

public sealed class ModelMetadataDto
{
    [Required]
    public required TypeDto Type { get; init; }
    [Required]
    public required Dictionary<string, IPropertyMetadataDto> PropertyMetadatas { get; init; }
}
