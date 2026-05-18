using FormsApi.Contract.ControlMetadata;

namespace FormsApi.Contract.MetadataCollection;

public sealed class PrimitivePropertyMetadataDto : IPropertyMetadataDto
{
    public IEnumerable<IControlMetadataDto>? Metadatas { get; init; }
}
