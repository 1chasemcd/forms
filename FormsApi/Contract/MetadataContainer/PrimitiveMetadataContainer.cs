using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.MetadataContainer;

public sealed record PrimitivePropertyMetadataContainer : PropertyMetadataContainer
{
    [Required]
    public required IEnumerable<PropertyMetadata.PropertyMetadata> Metadatas { get; init; }
}
