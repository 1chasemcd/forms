using System.Text.Json.Serialization;

namespace FormsApi.Contract.View;

[JsonPolymorphic(
    TypeDiscriminatorPropertyName = "$type",
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor
)]
[JsonDerivedType(typeof(FieldViewDto), "fieldView")]
[JsonDerivedType(typeof(CombinedViewDto), "combinedView")]
[JsonDerivedType(typeof(SubPropertyGridViewDto), "subPropertyGridView")]
public abstract record BaseViewDto
{
    public PropertyOrConstantDto? Title { get; init; }
    public int? Width { get; init; }
    public PropertyOrConstantDto? Enabled { get; init; }
    public PropertyOrConstantDto? Visible { get; init; }

}
