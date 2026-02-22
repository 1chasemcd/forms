using System.Collections;
using System.Linq.Expressions;
using FormsApi.Form.Primitives;
using FormsApi.Form.View;

namespace FormsApi.Builder.View;

public sealed class CombinedViewBuilder<TModel> : ViewBuilder<TModel>, IEnumerable
{
    private readonly IList<ViewBuilder<TModel>> _views = [];

    public CombinedViewBuilder(PropertyOrConstantBuilder<TModel, string>? title = null, FormElementSize? width = null)
    {
        Title = title;
        Width = width;
    }

    public CombinedViewBuilder(Expression<Func<TModel, string>> title, FormElementSize? width = null)
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
