using System.Collections;
using System.Linq.Expressions;

namespace FormsApi.Forms;

public sealed class CombinedViewBuilder<TModel> : ViewBuilder<TModel, CombinedViewBuilder<TModel>>, IEnumerable
{
    internal IReadOnlyList<IViewBuilder<TModel>> Views => _views;
    private readonly List<IViewBuilder<TModel>> _views = [];
    internal bool IsUnified { get; private set; }
    public CombinedViewBuilder(string? title = null)
    {
        if (title != null)
            Title = title;
    }

    public CombinedViewBuilder(Expression<Func<TModel, string?>> title)
    {
        Title = title;
    }
    IEnumerator IEnumerable.GetEnumerator() => Views.GetEnumerator();

    public void Add(IViewBuilder<TModel> view, int? width = null)
    {
        view.Width = width;
        _views.Add(view);
    }
    public CombinedViewBuilder<TModel> Unify()
    {
        IsUnified = true;
        return this;
    }
}
