using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract;

public sealed class RecalculateEventDto
{
    [Required]
    public required TypeDto Service { get; init; }
    [Required]
    public required string Method { get; init; }
}
