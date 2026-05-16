using System.ComponentModel.DataAnnotations;
using FormsApi.Definition.Primitives;

namespace FormsApi.Definition.Metadata;

public sealed class SubPropertyMetadataDto(TypeDto type) : IPropertyMetadataDto
{
    [Required]
    public TypeDto Type { get; init; } = type;
}
