using System.Collections;
using System.Linq.Expressions;
using FormsApi.Common;
using FormsApi.Contract;
using FormsApi.Contract.View;

namespace FormsApi.Forms;

public sealed class ControlViewBuilder<TModel> : ViewBuilder<TModel, ControlViewBuilder<TModel>>, IEnumerable
{
    internal IReadOnlyList<FormControl> ControlList => _controlList;
    private readonly List<FormControl> _controlList = [];

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
