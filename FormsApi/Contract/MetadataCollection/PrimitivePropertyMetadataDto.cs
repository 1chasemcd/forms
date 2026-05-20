using FormsApi.Contract.ControlMetadata;

namespace FormsApi.Contract.MetadataCollection;

public sealed record PrimitivePropertyMetadataDto : IPropertyMetadataDto
{
    public IEnumerable<BaseControlMetadataDto>? Metadatas { get; init; }
}
