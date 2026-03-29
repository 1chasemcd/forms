using System.ComponentModel.DataAnnotations;
using FormsApi.Definition.Primitives;

namespace FormsApi.Definition.View;

public sealed record class CombinedViewDefinition : IViewDefinition
{
    public PropertyOrConstant? Title { get; init; }
    public FormElementSize? Width { get; init; }
    [Required]
    public required IEnumerable<IViewDefinition> Views { get; init; }
}
