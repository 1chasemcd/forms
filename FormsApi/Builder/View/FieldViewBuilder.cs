using System.Collections;
using FormsApi.Builder.Field;
using FormsApi.Definition.Primitives;
using FormsApi.Definition.View;

namespace FormsApi.Builder.View;

public sealed class FieldViewBuilder<TModel> : ViewBuilder<TModel>, IFieldCollection<TModel>
{
    public FieldViewBuilder(PropertyOrConstantBuilder<TModel, string?>? title = null, FormElementSize? width = null)
    {
        Title = title;
        Width = width;
    }

    public IList<IFieldBuilder<TModel>> Fields { get; } = [];
    protected override FieldViewDefinition BuildImpl()
    {
        var view = new FieldViewDefinition
        {
            Fields = Fields.Select(x => x.Build())
        };

        return view;
    }
    public IEnumerator GetEnumerator() => Fields.GetEnumerator();
}
