using System;
using FormsApi.Definition.Primitives;

namespace FormsApi.Definition.Metadata;

public sealed record class RecalculateEventMetadata : BaseMetadataDefinition
{
    public required RecalculateEvent RecalculateEvent { get; init; }

}
