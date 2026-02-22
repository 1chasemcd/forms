using System.Collections;
using FormsApi.Builder.Field;
using FormsApi.Form.Primitives;
using FormsApi.Form.View;

namespace FormsApi.Builder.View;

public sealed class DataViewBuilder<TModel> : ViewBuilder<TModel>, IFieldCollection<TModel>
{
    public DataViewBuilder(PropertyOrConstantBuilder<TModel, string>? title = null, FormElementSize? width = null)
    {
        Title = title;
        Width = width;
    }

    public IList<BaseFieldBuilder<TModel>> Fields { get; } = [];
    protected override DataView BuildImpl()
    {
        var view = new DataView
        {
            Fields = Fields.Select(x => x.Build())
        };

        return view;
    }
    public IEnumerator GetEnumerator() => Fields.GetEnumerator();
}
