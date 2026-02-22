using System.Text.Json.Serialization;
using FormsApi.Form.Primitives;

namespace FormsApi.Form.View;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(DataView), "dataview")]
[JsonDerivedType(typeof(CombinedView), "combinedview")]
[JsonDerivedType(typeof(GridView), "gridview")]
[JsonDerivedType(typeof(SubPropertyGridView), "subpropertygridview")]
[JsonDerivedType(typeof(RepositoryGridView), "repositorygridview")]
public abstract record class BaseView
{
    public PropertyOrConstant? Title { get; init; }
    public FormElementSize? Width { get; init; }
}
