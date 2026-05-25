using System.Collections;
using System.Linq.Expressions;
using FormsApi.Contract;

namespace FormsApi.Forms;

public sealed class ControlViewBuilder<TModel> : ViewBuilder<TModel, ControlViewBuilder<TModel>>, IEnumerable
{
    internal IReadOnlyList<FormControlInfoContainer> ControlList => _controlList;
    private readonly List<FormControlInfoContainer> _controlList = [];

    public ControlViewBuilder(string? title = null)
    {
        if (title != null)
            Title = title;
    }

    public ControlViewBuilder(Expression<Func<TModel, string?>> title)
    {
        Title = title;
    }

    public void Add(Expression<Func<TModel, object?>> selector, int? width = null)
    {
        _controlList.Add(new(selector.GetPropertyName(), width));
    }

    public IEnumerator GetEnumerator() => ControlList.GetEnumerator();
}
