using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.View;

public sealed record CombinedViewDto : BaseViewDto
{
    [Required]
    public required IReadOnlyList<int> ViewIds { get; init; }
    [Required]
    public bool Unify { get; init; }
}
