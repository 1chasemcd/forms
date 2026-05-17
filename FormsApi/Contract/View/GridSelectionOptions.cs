using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.View;

public sealed record class GridSelectionOptions
{
    [Required]
    public GridSelectionType SelectionType { get; init; }
    [Required]
    public required string SelectionProperty { get; init; }
}

public enum GridSelectionType
{
    Single,
    Multiple
}
