using System.Collections;
using System.Linq.Expressions;
using FormsApi.Contract;

namespace FormsApi.Forms;

public sealed class FieldViewBuilder<TModel> : ViewBuilder<TModel, FieldViewBuilder<TModel>>, IEnumerable
{
    internal IReadOnlyList<FormFieldInfoContainer> FieldList => _fieldList;
    private readonly List<FormFieldInfoContainer> _fieldList = [];

    public FieldViewBuilder(string? title = null)
    {
        if (title != null)
            Title = title;
    }

    public FieldViewBuilder(Expression<Func<TModel, string?>> title)
    {
        Title = title;
    }

    public void Add(Expression<Func<TModel, object?>> selector, int? width = null)
    {
        _fieldList.Add(new(selector.GetPropertyName(), width));
    }

    public IEnumerator GetEnumerator() => FieldList.GetEnumerator();
}
