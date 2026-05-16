using System.ComponentModel.DataAnnotations;

namespace FormsApi.Definition.View;

public sealed record class CombinedViewDto : BaseViewDto
{
    [Required]
    public required IEnumerable<BaseViewDto> Views { get; init; }
    [Required]
    public bool Unify { get; init; }
}
