using System.ComponentModel.DataAnnotations;
using FormsApi.Definition.Primitives;

namespace FormsApi.Definition.Field;

public sealed record class ButtonDefinition : IFieldDefinition
{
    [Required]
    public required PropertyOrConstant Label { get; init; }
    public PropertyOrConstant? Hidden { get; init; }
    public FormElementSize? Width { get; init; }
    [Required]
    public required RecalculateEvent RecalculateEvent { get; init; }
    public PropertyOrConstant? Disabled { get; init; }
}
