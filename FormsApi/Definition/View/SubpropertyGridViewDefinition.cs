using System.ComponentModel.DataAnnotations;
using FormsApi.Definition.Field;
using FormsApi.Definition.Metadata;
using FormsApi.Definition.Primitives;

namespace FormsApi.Definition.View;

public sealed record class SubPropertyGridViewDefinition : BaseViewDefinition
{
    [Required]
    public required IEnumerable<FieldDefinition> Fields { get; init; }
    [Required]
    public required string IdProperty { get; init; }
    [Required]
    public required string SubPropertyName { get; init; }
    public PropertyOrConstant? CanAdd { get; init; }
    public PropertyOrConstant? CanEdit { get; init; }
    public PropertyOrConstant? CanEditRow { get; init; }
    public PropertyOrConstant? CanDelete { get; init; }
    public PropertyOrConstant? CanDeleteRow { get; init; }
    public FormDefinition? EditForm { get; init; }
    public GridSelectionOptions? SelectionOptions { get; init; }
}
