namespace FormsApi.WebForm.View;

public abstract class View
{
    public PropertyOrConstant<string>? Title { get; init; }
    public FormElementSize Width { get; init; }
}
