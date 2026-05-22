using FormsApi.Common;
using FormsApi.Contract.View;
using FormsApi.Forms.Services;
using Microsoft.AspNetCore.Http;

namespace FormsApi.Forms;

public interface IForm
{
    Type ModelType { get; }
    IReadOnlyList<BaseViewDto> ProvideBuilder(IFormBuilderService builder);
}
public abstract class Form<TModel> : IForm
{
    public Type ModelType => typeof(TModel);
    protected internal abstract IView<TModel> View { get; }
    public IReadOnlyList<BaseViewDto> ProvideBuilder(IFormBuilderService builder) => builder.BuildFormIntoViews(this);

}
