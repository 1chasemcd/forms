using System;
using System.ComponentModel.DataAnnotations;

namespace FormsApi.Definition.Primitives;

public sealed record class GridSelectionOptions
{
    [Required]
    public GridSelectionType SelectionType { get; init; }
    [Required]
    public required string SelectionProperty { get; init; }
}
