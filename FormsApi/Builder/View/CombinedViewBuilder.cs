using System.Collections;
using System.Linq.Expressions;
using FormsApi.Definition.Primitives;
using FormsApi.Definition.View;

namespace FormsApi.Builder.View;

public sealed class CombinedViewBuilder<TModel> : ViewBuilder<TModel>, IEnumerable
{
    private readonly IList<ViewBuilder<TModel>> _views = [];

    public CombinedViewBuilder(PropertyOrConstantBuilder<TModel, string?>? title = null, int? width = null)
    {
        Title = title;
        Width = width;
    }

    public CombinedViewBuilder(Expression<Func<TModel, string?>> title, int? width = null)
    {
        Title = title;
        Width = width;
    }

    protected override CombinedViewDefinition BuildImpl()
    {
        var view = new CombinedViewDefinition
        {
            Views = _views.Select(x => x.Build())
        };

        return view;
    }

    public void Add(ViewBuilder<TModel> view) => _views.Add(view);
    IEnumerator IEnumerable.GetEnumerator() => _views.GetEnumerator();
}
