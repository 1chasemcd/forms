using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.View;

public sealed record SubPropertyGridView : View
{
    [Required]
    public required IReadOnlyList<FormControlInfoContainer> Controls { get; init; }
    [Required]
    public required string SubProperty { get; init; }
    [Required]
    public required string IdProperty { get; init; }
    public PropertyOrConstant? CanAdd { get; init; }
    public PropertyOrConstant? CanEdit { get; init; }
    public PropertyOrConstant? CanDelete { get; init; }
    public PropertyOrConstant? CanEditRow { get; init; }
    public PropertyOrConstant? CanDeleteRow { get; init; }
    public GridSelectionOptions? GridSelectionOptions { get; init; }
    public int? EditViewId { get; init; }
    public TypeDto? ProjectionModelType { get; init; }
    public string? ProjectionModelIdProperty { get; init; }
}
