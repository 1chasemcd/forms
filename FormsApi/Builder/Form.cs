using FormsApi.Builder.View;
using FormsApi.Definition.View;

namespace FormsApi.Builder;

public abstract class Form
{
    internal abstract BaseViewDto GetView();
    internal abstract Type GetModelType();

}
public abstract class Form<TModel> : Form
{
    internal override BaseViewDto GetView() => View.Build();
    internal override Type GetModelType() => typeof(TModel);
    protected abstract BaseView<TModel> View { get; }
}
