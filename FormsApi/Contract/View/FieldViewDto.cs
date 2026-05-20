using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.View;

public sealed record FieldViewDto : BaseViewDto
{
    [Required]
    public required IEnumerable<FormControlLayoutDto> Fields { get; init; }
}
