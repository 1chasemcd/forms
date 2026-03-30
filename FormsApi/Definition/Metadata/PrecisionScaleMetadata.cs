using System;
using FormsApi.Definition.Primitives;

namespace FormsApi.Definition.Metadata;

public sealed record class PrecisionScaleMetadata : BaseMetadataDefinition
{
    public PropertyOrConstant? Precision { get; init; }
    public PropertyOrConstant? Scale { get; init; }
}
