using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.View;

public sealed record ControlView : View
{
    [Required]
    public required IReadOnlyList<FormControlInfoContainer> Controls { get; init; }
}
