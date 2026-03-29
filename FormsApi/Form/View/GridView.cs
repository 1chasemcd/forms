using System.ComponentModel.DataAnnotations;
using FormsApi.Form.Field;
using FormsApi.Form.Primitives;

namespace FormsApi.Form.View;

public abstract record class GridView : BaseView
{
    [Required]
    public required IEnumerable<BaseInput> Fields { get; init; }
    [Required]
    public required string IdProperty { get; init; }
    public PropertyOrConstant? CanAdd { get; init; }
    public PropertyOrConstant? CanEdit { get; init; }
    public PropertyOrConstant? CanEditRow { get; init; }
    public PropertyOrConstant? CanDelete { get; init; }
    public PropertyOrConstant? CanDeleteRow { get; init; }
    public FormDefinition? EditForm { get; init; }
}
