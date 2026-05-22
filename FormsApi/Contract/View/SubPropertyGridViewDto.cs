using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.View;

public sealed record SubPropertyGridViewDto : BaseViewDto
{
    [Required]
    public required IReadOnlyList<FormControlLayoutDto> Controls { get; init; }
    [Required]
    public required string SubProperty { get; init; }
    [Required]
    public required string IdProperty { get; init; }
    public PropertyOrConstantDto? CanAdd { get; init; }
    public PropertyOrConstantDto? CanEdit { get; init; }
    public PropertyOrConstantDto? CanDelete { get; init; }
    public PropertyOrConstantDto? CanEditRow { get; init; }
    public PropertyOrConstantDto? CanDeleteRow { get; init; }
    public GridSelectionOptions? GridSelectionOptions { get; init; }
    public int? EditViewId { get; init; }
    public TypeDto? ProjectionModelType { get; init; }
    public string? ProjectionModelIdProperty { get; init; }
}
