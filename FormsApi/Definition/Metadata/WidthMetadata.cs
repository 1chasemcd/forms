using FormsApi.Definition.Primitives;

namespace FormsApi.Definition.Metadata;

public sealed record class WidthMetadata : BaseMetadataDefinition
{
    public required FormElementSize Width { get; init; }
}
