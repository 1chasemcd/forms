using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.ModelMetadata;

public sealed class EnumerableMetadataDto(TypeDto type) : IPropertyMetadataDto
{
    [Required]
    public TypeDto Type { get; init; } = type;
}
