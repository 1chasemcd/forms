using System.Text.Json.Serialization;

namespace FormsApi.Definition.Metadata;

[JsonPolymorphic(
    TypeDiscriminatorPropertyName = "$type",
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor
)]
[JsonDerivedType(typeof(EnumerableMetadataDto), nameof(EnumerableMetadataDto))]
[JsonDerivedType(typeof(SubPropertyMetadataDto), nameof(SubPropertyMetadataDto))]
[JsonDerivedType(typeof(PrimitiveMetadataDto), nameof(PrimitiveMetadataDto))]
public interface IPropertyMetadataDto;
