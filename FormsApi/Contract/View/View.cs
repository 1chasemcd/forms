using System.Text.Json.Serialization;

namespace FormsApi.Contract.View;

[JsonPolymorphic(
    TypeDiscriminatorPropertyName = "$type",
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor
)]
[JsonDerivedType(typeof(ControlView), "fieldView")]
[JsonDerivedType(typeof(CombinedView), "combinedView")]
[JsonDerivedType(typeof(SubPropertyGridView), "subPropertyGridView")]
public abstract record View
{
    public PropertyOrConstant? Title { get; init; }
    public int? Width { get; init; }
    public PropertyOrConstant? Visible { get; init; }

}
