using FormsApi.Contract.ControlMetadata;

namespace FormsApi.Contract.ModelMetadata;

public sealed class PrimitiveMetadataDto : IPropertyMetadataDto
{
    public IEnumerable<IInputMetadataDto>? Metadatas { get; init; }
}
