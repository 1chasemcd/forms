using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.View;

public sealed record TableSelectionOptions
{
    [Required]
    public TableSelectionType SelectionType { get; init; }
    [Required]
    public required string SelectionProperty { get; init; }
}

public enum TableSelectionType
{
    Single,
    Multiple
}
