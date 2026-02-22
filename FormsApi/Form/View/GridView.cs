using FormsApi.Form.Field;
using FormsApi.Form.Primitives;

namespace FormsApi.Form.View;

public abstract record class GridView : BaseView
{
    public required IEnumerable<BaseField> Columns { get; init; }
    public PropertyOrConstant? CanAdd { get; init; }
    public PropertyOrConstant? CanEdit { get; init; }
    public PropertyOrConstant? CanDelete { get; init; }
    public FormModel? EditForm { get; init; }
}
