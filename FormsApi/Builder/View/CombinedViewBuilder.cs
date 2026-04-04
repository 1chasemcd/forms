using System.Collections;
using System.Linq.Expressions;
using FormsApi.Definition.Primitives;
using FormsApi.Definition.View;

namespace FormsApi.Builder.View;

public sealed class CombinedViewBuilder<TModel> : ViewBuilder<TModel>, IEnumerable
{
    private readonly IList<ViewBuilder<TModel>> _views = [];
    public bool Unify { get; set; }

    public CombinedViewBuilder(PropertyOrConstantBuilder<TModel, string?>? title = null, int? width = null, bool unify = false)
    {
        Title = title;
        Width = width;
        Unify = unify;
    }

    public CombinedViewBuilder(Expression<Func<TModel, string?>> title, int? width = null, bool unify = false)
    {
        Title = title;
        Width = width;
        Unify = unify;
    }

    protected override CombinedViewDefinition BuildImpl()
    {
        var view = new CombinedViewDefinition
        {
            Views = _views.Select(x => x.Build()),
            Unify = Unify || Title != null
        };

        return view;
    }

    public void Add(ViewBuilder<TModel> view) => _views.Add(view);
    IEnumerator IEnumerable.GetEnumerator() => _views.GetEnumerator();
}
