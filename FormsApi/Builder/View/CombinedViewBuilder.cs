using System.Collections;
using FormsApi.Form;
using FormsApi.Form.View;

namespace FormsApi.Builder.View;

public sealed class CombinedViewBuilder<TModel> : ViewBuilder<TModel>, IEnumerable
{
    private readonly IList<ViewBuilder<TModel>> _views = [];

    public CombinedViewBuilder(PropertyOrConstantBuilder<TModel, string>? title = null, FormElementSize width = default)
    {
        Title = title;
        Width = width;
    }

    protected override CombinedView BuildImpl()
    {
        var view = new CombinedView
        {
            Views = _views.Select(x => x.Build())
        };

        return view;
    }

    public void Add(ViewBuilder<TModel> view) => _views.Add(view);
    IEnumerator IEnumerable.GetEnumerator() => _views.GetEnumerator();
}
