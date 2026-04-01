using FormsApi.Definition.Primitives;
using FormsApi.Definition.View;

namespace FormsApi.Builder.View;

public abstract class ViewBuilder<TModel> : IBuildable<BaseViewDefinition>
{
    public PropertyOrConstantBuilder<TModel, string?>? Title { get; set; }
    public FormElementSize? Width { get; set; }
    public BaseViewDefinition Build()
    {
        BaseViewDefinition baseView = BuildImpl();
        return baseView with
        {
            Title = Title?.Build(),
            Width = Width
        };
    }

    protected abstract BaseViewDefinition BuildImpl();
}
