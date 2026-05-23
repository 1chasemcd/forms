using FormsApi.Contract.PropertyMetadata;

namespace FormsApi.Contract.MetadataContainer;

public sealed record PrimitivePropertyMetadataContainer : PropertyMetadataContainer
{
    public IEnumerable<PropertyMetadata.PropertyMetadata>? Metadatas { get; init; }
}
