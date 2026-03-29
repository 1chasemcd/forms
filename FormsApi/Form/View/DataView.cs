using System.ComponentModel.DataAnnotations;
using FormsApi.Form.Field;

namespace FormsApi.Form.View;

public sealed record class DataView : BaseView
{
    [Required]
    public required IEnumerable<BaseField> Fields { get; init; }
}
