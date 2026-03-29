using System.ComponentModel.DataAnnotations;
using FormsApi.Definition.Primitives;

namespace FormsApi.Definition.Field;

public sealed record class CurrencyInputDefinition : IFieldDefinition, IModelableField
{
    [Required]
    public required PropertyOrConstant Label { get; init; }
    public PropertyOrConstant? Hidden { get; init; }
    public FormElementSize? Width { get; init; }
    [Required]
    public required string Property { get; init; }
    public RecalculateEvent? RecalculateEvent { get; init; }
    public PropertyOrConstant? Required { get; init; }
    public PropertyOrConstant? Disabled { get; init; }
    public PropertyOrConstant? MaxValue { get; init; }
    public PropertyOrConstant? MinValue { get; init; }
}
