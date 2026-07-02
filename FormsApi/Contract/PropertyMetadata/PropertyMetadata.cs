using System.Text.Json.Serialization;

namespace FormsApi.Contract.PropertyMetadata;

[JsonPolymorphic(
    TypeDiscriminatorPropertyName = "$type",
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor
)]
[JsonDerivedType(typeof(FieldTypeMetadata), "fieldType")]
[JsonDerivedType(typeof(EnabledMetadata), "enabled")]
[JsonDerivedType(typeof(ServiceMethodMetadata), "formServiceMethod")]
[JsonDerivedType(typeof(LabelMetadata), "label")]
[JsonDerivedType(typeof(MaxLengthMetadata), "maxLength")]
[JsonDerivedType(typeof(MaxValueMetadata), "maxValue")]
[JsonDerivedType(typeof(MinValueMetadata), "minValue")]
[JsonDerivedType(typeof(PrecisionMetadata), "precision")]
[JsonDerivedType(typeof(RequiredMetadata), "required")]
[JsonDerivedType(typeof(ScaleMetadata), "scale")]
[JsonDerivedType(typeof(VisibleMetadata), "visible")]
public abstract record PropertyMetadata;
