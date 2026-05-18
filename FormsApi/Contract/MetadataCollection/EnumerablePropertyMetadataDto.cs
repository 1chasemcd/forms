using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.MetadataCollection;

public sealed class EnumerablePropertyMetadataDto(TypeDto type) : IPropertyMetadataDto
{
    [Required]
    public TypeDto Type { get; init; } = type;
}
