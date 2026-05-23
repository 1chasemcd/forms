using System.Text.Json.Serialization;

namespace FormsApi.Contract.PropertyMetadata;

[JsonPolymorphic(
    TypeDiscriminatorPropertyName = "$type",
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor
)]
[JsonDerivedType(typeof(ControlTypeMetadata), "controlType")]
[JsonDerivedType(typeof(EnabledMetadata), "enabled")]
[JsonDerivedType(typeof(FormServiceMethodMetadata), "formServiceMethod")]
[JsonDerivedType(typeof(LabelMetadata), "label")]
[JsonDerivedType(typeof(MaxLengthMetadata), "maxLength")]
[JsonDerivedType(typeof(MaxValueMetadata), "maxValue")]
[JsonDerivedType(typeof(MinValueMetadata), "minValue")]
[JsonDerivedType(typeof(PrecisionMetadata), "precision")]
[JsonDerivedType(typeof(RequiredMetadata), "required")]
[JsonDerivedType(typeof(ScaleMetadata), "scale")]
[JsonDerivedType(typeof(VisibleMetadata), "visible")]
public abstract record PropertyMetadata;
