using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.View;

public sealed record SubPropertyGridViewDto : BaseViewDto
{
    [Required]
    public required IEnumerable<FormControlLayoutDto> Fields { get; init; }
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
