using System.ComponentModel.DataAnnotations;

namespace FormsApi.Definition.Field;

public sealed record class FieldDto
{
    [Required]
    public required string Property { get; init; }
    public int? Width { get; init; }
}
