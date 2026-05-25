using FormsApi.Contract.View;
using FormsApi.Forms.Services;

namespace FormsApi.Forms;

public interface IForm
{
    Type ModelType { get; }
    IReadOnlyList<View> ProvideBuilder(IFormBuilderService builder);
}
public abstract class Form<TModel> : IForm
{
    public Type ModelType => typeof(TModel);
    protected internal abstract IViewBuilder<TModel> View { get; }
    public IReadOnlyList<View> ProvideBuilder(IFormBuilderService builder) => builder.BuildFormIntoViews(this);

}
