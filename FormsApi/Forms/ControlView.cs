using System.Collections;
using System.Linq.Expressions;
using FormsApi.Common;
using FormsApi.Contract;
using FormsApi.Contract.View;

namespace FormsApi.Forms;

public sealed class ControlView<TModel> : View<TModel, ControlView<TModel>>, IEnumerable
{
    internal IReadOnlyList<FormControlLayoutDto> ControlList => _controlList;
    private readonly List<FormControlLayoutDto> _controlList = [];

    public ControlView(string? title = null)
    {
        if (title != null)
            Title = new ConstantDto(title);
    }

    public ControlView(Expression<Func<TModel, string?>> title)
    {
        Title = new PropertyDto(title.GetPropertyName());
    }

    public void Add(Expression<Func<TModel, object?>> selector, int? width = null)
    {
        _controlList.Add(new(selector.GetPropertyName(), width));
    }

    public IEnumerator GetEnumerator() => ControlList.GetEnumerator();
}
