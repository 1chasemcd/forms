using System.ComponentModel.DataAnnotations;
using FormsApi.Form.Field;

namespace FormsApi.Form.View;

public sealed record class DataView : BaseView, IFieldView
{
    [Required]
    public required IEnumerable<BaseField> Fields { get; init; }
}
