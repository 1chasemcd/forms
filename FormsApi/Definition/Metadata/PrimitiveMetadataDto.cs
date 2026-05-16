using FormsApi.Definition.InputMetadata;

namespace FormsApi.Definition.Metadata;

public sealed class PrimitiveMetadataDto : IPropertyMetadataDto
{
    public IEnumerable<IInputMetadataDto>? Metadatas { get; init; }
}
