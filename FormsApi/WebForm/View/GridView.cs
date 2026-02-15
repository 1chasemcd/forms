using System;
using FormsApi.WebForm.Field;

namespace FormsApi.WebForm.View;

public abstract class GridView : View
{
    public required IEnumerable<BaseField> Columns { get; init; }
    public PropertyOrConstant<bool>? CanAdd { get; init; }
    public PropertyOrConstant<bool>? CanEdit { get; init; }
    public PropertyOrConstant<bool>? CanDelete { get; init; }
    public WebForm? EditForm { get; init; }
}
