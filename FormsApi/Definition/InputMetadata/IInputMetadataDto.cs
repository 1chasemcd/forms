using System.Text.Json.Serialization;

namespace FormsApi.Definition.InputMetadata;

[JsonPolymorphic(
    TypeDiscriminatorPropertyName = "$type",
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor
)]
[JsonDerivedType(typeof(EnabledMetadataDto), nameof(EnabledMetadataDto))]
[JsonDerivedType(typeof(InputTypeMetadataDto), nameof(InputTypeMetadataDto))]
[JsonDerivedType(typeof(MaxLengthMetadataDto), nameof(MaxLengthMetadataDto))]
[JsonDerivedType(typeof(MaxValueMetadataDto), nameof(MaxValueMetadataDto))]
[JsonDerivedType(typeof(MinValueMetadataDto), nameof(MinValueMetadataDto))]
[JsonDerivedType(typeof(PrecisionMetadataDto), nameof(PrecisionMetadataDto))]
[JsonDerivedType(typeof(RecalculateEventMetadataDto), nameof(RecalculateEventMetadataDto))]
[JsonDerivedType(typeof(RequiredMetadataDto), nameof(RequiredMetadataDto))]
[JsonDerivedType(typeof(ScaleMetadataDto), nameof(ScaleMetadataDto))]
[JsonDerivedType(typeof(VisibleMetadataDto), nameof(VisibleMetadataDto))]
public interface IInputMetadataDto;
