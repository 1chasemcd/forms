using System.Text.Json.Serialization;

namespace FormsApi.Contract.View;

[JsonPolymorphic(
    TypeDiscriminatorPropertyName = "$type",
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor
)]
[JsonDerivedType(typeof(FieldView), "fieldView")]
[JsonDerivedType(typeof(CombinedView), "combinedView")]
[JsonDerivedType(typeof(SubPropertyTableView), "subPropertyTableView")]
public abstract record View
{
    public FormValueRef? Title { get; init; }
    public int? Width { get; init; }
    public FormValueRef? Visible { get; init; }

}
