using System.Text.Json.Serialization;

namespace FormsApi.Contract.View;

[JsonPolymorphic(
    TypeDiscriminatorPropertyName = "$type",
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor
)]
[JsonDerivedType(typeof(FieldViewDto), nameof(FieldViewDto))]
[JsonDerivedType(typeof(CombinedViewDto), nameof(CombinedViewDto))]
[JsonDerivedType(typeof(SubPropertyGridViewDefinition), nameof(SubPropertyGridViewDefinition))]
public abstract record class BaseViewDto
{
    public PropertyOrConstantDto? Title { get; init; }
    public int? Width { get; init; }
    public PropertyOrConstantDto? Enabled { get; init; }
    public PropertyOrConstantDto? Visible { get; init; }

}
