using System.Text.Json.Serialization;

namespace FormsApi.Contract.MetadataCollection;

[JsonPolymorphic(
    TypeDiscriminatorPropertyName = "$type",
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor
)]
[JsonDerivedType(typeof(EnumerablePropertyMetadataDto), "enumerable")]
[JsonDerivedType(typeof(SubPropertyMetadataDto), "subproperty")]
[JsonDerivedType(typeof(PrimitivePropertyMetadataDto), "primitive")]
public abstract record IPropertyMetadataDto;
