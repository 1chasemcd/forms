using System;
using System.Text.Json.Serialization;

namespace FormsApi.Definition.Metadata;

[JsonPolymorphic(
    TypeDiscriminatorPropertyName = "$type",
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor
)]
[JsonDerivedType(typeof(EnabledMetadata), "enabled")]
[JsonDerivedType(typeof(LabelMetadata), "label")]
[JsonDerivedType(typeof(MaxLengthMetadata), "maxlength")]
[JsonDerivedType(typeof(PrecisionScaleMetadata), "precisionscale")]
[JsonDerivedType(typeof(RecalculateEventMetadata), "recalculateevent")]
[JsonDerivedType(typeof(RequiredMetadata), "required")]
[JsonDerivedType(typeof(ValueRangeMetadata), "valuerange")]
[JsonDerivedType(typeof(VisibleMetadata), "visible")]
[JsonDerivedType(typeof(WidthMetadata), "width")]
public abstract record class BaseMetadataDefinition;
