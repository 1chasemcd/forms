using System.Text.Json.Serialization;
using FormsApi.Definition.Primitives;

namespace FormsApi.Definition.View;

[JsonPolymorphic(
    TypeDiscriminatorPropertyName = "$type",
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor
)]
[JsonDerivedType(typeof(FieldViewDefinition), "fieldview")]
[JsonDerivedType(typeof(CombinedViewDefinition), "combinedview")]
[JsonDerivedType(typeof(SubPropertyGridViewDefinition), "subpropertygridview")]
public interface IViewDefinition
{
    public PropertyOrConstant? Title { get; init; }
    public FormElementSize? Width { get; init; }
}
