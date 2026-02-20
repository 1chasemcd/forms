using FormsApi.Form.Field;

namespace FormsApi.Form.View;

public abstract record class GridView : BaseView
{
    public required IEnumerable<BaseField> Columns { get; init; }
    public PropertyOrConstant<bool>? CanAdd { get; init; }
    public PropertyOrConstant<bool>? CanEdit { get; init; }
    public PropertyOrConstant<bool>? CanDelete { get; init; }
    public FormModel? EditForm { get; init; }
}
