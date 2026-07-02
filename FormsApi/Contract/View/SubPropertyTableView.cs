using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.View;

public sealed record SubPropertyTableView : View
{
    [Required]
    public required IReadOnlyList<FormFieldInfoContainer> Fields { get; init; }
    [Required]
    public required string SubProperty { get; init; }
    [Required]
    public required string IdProperty { get; init; }
    public FormValueRef? CanAdd { get; init; }
    public FormValueRef? CanEdit { get; init; }
    public FormValueRef? CanDelete { get; init; }
    public FormValueRef? CanEditRow { get; init; }
    public FormValueRef? CanDeleteRow { get; init; }
    public TableSelectionOptions? TableSelectionOptions { get; init; }
    public int? EditViewId { get; init; }
    public TypeDto? ProjectionModelType { get; init; }
    public string? ProjectionModelIdProperty { get; init; }
}
