namespace FormsApi.Form.View;

public abstract record class BaseView
{
    public PropertyOrConstant<string>? Title { get; init; }
    public FormElementSize Width { get; init; }
}
