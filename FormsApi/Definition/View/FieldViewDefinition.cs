using System.ComponentModel.DataAnnotations;
using FormsApi.Definition.Field;
using FormsApi.Definition.Primitives;

namespace FormsApi.Definition.View;

public sealed record class FieldViewDefinition : IViewDefinition
{
    public PropertyOrConstant? Title { get; init; }
    public FormElementSize? Width { get; init; }
    [Required]
    public required IEnumerable<IFieldDefinition> Fields { get; init; }
}
