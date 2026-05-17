using FormsApi.Contract.View;

namespace FormsApi.Forms;

public interface IForm
{
    BaseViewDto GetView();
    Type GetModelType();

}
public abstract class Form<TModel> : IForm
{
    public BaseViewDto GetView() => View.Build();
    public Type GetModelType() => typeof(TModel);
    protected abstract IFormView<TModel> View { get; }
}
