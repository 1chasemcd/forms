using System.Text.Json.Serialization;

namespace FormsApi.Contract.ControlMetadata;

[JsonPolymorphic(
    TypeDiscriminatorPropertyName = "$type",
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor
)]
[JsonDerivedType(typeof(ControlTypeMetadataDto), "controlType")]
[JsonDerivedType(typeof(EnabledMetadataDto), "enabled")]
[JsonDerivedType(typeof(FormServiceMethodMetadataDto), "formServiceMethod")]
[JsonDerivedType(typeof(LabelMetadataDto), "label")]
[JsonDerivedType(typeof(MaxLengthMetadataDto), "maxLength")]
[JsonDerivedType(typeof(MaxValueMetadataDto), "maxValue")]
[JsonDerivedType(typeof(MinValueMetadataDto), "minValue")]
[JsonDerivedType(typeof(PrecisionMetadataDto), "precision")]
[JsonDerivedType(typeof(RequiredMetadataDto), "required")]
[JsonDerivedType(typeof(ScaleMetadataDto), "scale")]
[JsonDerivedType(typeof(VisibleMetadataDto), "visible")]
public abstract record BaseControlMetadataDto;
