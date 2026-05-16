using System.ComponentModel.DataAnnotations;
using FormsApi.Definition.Field;
using FormsApi.Definition.Primitives;

namespace FormsApi.Definition.View;

public sealed record class SubPropertyGridViewDefinition : BaseViewDto
{
    [Required]
    public required IEnumerable<FieldDto> Fields { get; init; }
    [Required]
    public required string SubProperty { get; init; }
    [Required]
    public required string IdProperty { get; init; }
    public FormDto? EditForm { get; init; }
    public PropertyOrConstantDto? CanAdd { get; init; }
    public PropertyOrConstantDto? CanEdit { get; init; }
    public PropertyOrConstantDto? CanDelete { get; init; }
    public PropertyOrConstantDto? CanEditRow { get; init; }
    public PropertyOrConstantDto? CanDeleteRow { get; init; }
    public GridSelectionOptions? GridSelectionOptions { get; init; }

}
