using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract;

public sealed record class FormControlLayoutDto
{
    [Required]
    public required string PropertyName { get; init; }
    public int? Width { get; init; }
}
