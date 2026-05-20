using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.MetadataCollection;

public sealed record ModelMetadataCollectionDto
{
    [Required]
    public required TypeDto Type { get; init; }
    [Required]
    public required Dictionary<string, IPropertyMetadataDto> PropertyMetadatas { get; init; }
}
