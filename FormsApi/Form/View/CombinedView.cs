using System.ComponentModel.DataAnnotations;

namespace FormsApi.Form.View;

public sealed record class CombinedView : BaseView
{
    [Required]
    public required IEnumerable<BaseView> Views { get; init; }
}
