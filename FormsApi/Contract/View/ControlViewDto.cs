using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.View;

public sealed record ControlViewDto : BaseViewDto
{
    [Required]
    public required IReadOnlyList<FormControlLayoutDto> Controls { get; init; }
}
