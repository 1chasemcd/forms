using FormsApi.Form.Primitives;

namespace FormsApi.Form.View;

public abstract record class BaseView
{
    public PropertyOrConstant? Title { get; init; }
    public FormElementSize? Width { get; init; }
}
