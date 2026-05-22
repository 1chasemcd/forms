using System.Collections;
using System.Linq.Expressions;
using FormsApi.Common;
using FormsApi.Contract;
using FormsApi.Contract.View;

namespace FormsApi.Forms;

public sealed class CombinedView<TModel> : View<TModel, CombinedView<TModel>>, IEnumerable
{
    internal IReadOnlyList<IView<TModel>> Views => _views;
    private readonly List<IView<TModel>> _views = [];
    internal bool IsUnified { get; private set; }
    public CombinedView(string? title = null)
    {
        if (title != null)
            Title = new ConstantDto(title);
    }

    public CombinedView(Expression<Func<TModel, string?>> title)
    {
        Title = new PropertyDto(title.GetPropertyName());
    }
    IEnumerator IEnumerable.GetEnumerator() => Views.GetEnumerator();

    public void Add(IView<TModel> view, int? width = null)
    {
        view.Width = width;
        _views.Add(view);
    }
    public CombinedView<TModel> Unify()
    {
        IsUnified = true;
        return this;
    }
}
