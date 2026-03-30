using System;
using FormsApi.Definition.Primitives;

namespace FormsApi.Definition.Metadata;

public sealed record class ValueRangeMetadata : BaseMetadataDefinition
{
    public PropertyOrConstant? MinValue { get; init; }
    public PropertyOrConstant? MaxValue { get; init; }
}
