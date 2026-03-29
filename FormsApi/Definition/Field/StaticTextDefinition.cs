using System.ComponentModel.DataAnnotations;
using FormsApi.Definition.Primitives;

namespace FormsApi.Definition.Field;

public sealed record class StaticTextDefinition : IFieldDefinition
{
    [Required]
    public required PropertyOrConstant Label { get; init; }
    public PropertyOrConstant? Hidden { get; init; }
    public FormElementSize? Width { get; init; }
    [Required]
    public required PropertyOrConstant Value { get; init; }
}
