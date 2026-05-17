using System.Text.Json.Serialization;

namespace FormsApi.Contract.ModelMetadata;

[JsonPolymorphic(
    TypeDiscriminatorPropertyName = "$type",
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor
)]
[JsonDerivedType(typeof(EnumerableMetadataDto), "enumerable")]
[JsonDerivedType(typeof(SubPropertyMetadataDto), "subproperty")]
[JsonDerivedType(typeof(PrimitiveMetadataDto), "primitive")]
public interface IPropertyMetadataDto;
