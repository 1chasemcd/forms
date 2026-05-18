using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.MetadataCollection;

public sealed class SubPropertyMetadataDto(TypeDto type) : IPropertyMetadataDto
{
    [Required]
    public TypeDto Type { get; init; } = type;
}
