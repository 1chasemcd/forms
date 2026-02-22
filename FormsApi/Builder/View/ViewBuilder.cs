using FormsApi.Form.Primitives;
using FormsApi.Form.View;

namespace FormsApi.Builder.View;

public abstract class ViewBuilder<TModel>
{
    public PropertyOrConstantBuilder<TModel, string>? Title { get; set; }
    public FormElementSize? Width { get; set; }
    internal BaseView Build()
    {
        BaseView baseView = BuildImpl();
        return baseView with
        {
            Title = Title?.Build(),
            Width = Width
        };
    }

    protected abstract BaseView BuildImpl();
}
