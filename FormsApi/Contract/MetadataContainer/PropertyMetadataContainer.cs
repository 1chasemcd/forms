using System.Text.Json.Serialization;

namespace FormsApi.Contract.MetadataContainer;

[JsonPolymorphic(
    TypeDiscriminatorPropertyName = "$type",
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor
)]
[JsonDerivedType(typeof(ArrayMetadataContainer), "enumerable")]
[JsonDerivedType(typeof(SubPropertyMetadataContainer), "subproperty")]
[JsonDerivedType(typeof(PrimitivePropertyMetadataContainer), "primitive")]
public abstract record PropertyMetadataContainer;
