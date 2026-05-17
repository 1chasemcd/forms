using System.Collections;
using System.Linq.Expressions;
using FormsApi.Common;
using FormsApi.Contract.View;

namespace FormsApi.Forms;

public sealed class FieldView<TModel> : BaseView<TModel, FieldView<TModel>>, IEnumerable
{
    public FieldList<TModel> Fields { get; set; } = [];

    public FieldView(PropertyOrConstant<TModel, string?>? title = null)
    {
        Title = title;
    }

    public FieldView(Expression<Func<TModel, string?>> title)
    {
        Title = title;
    }

    protected override FieldViewDto BuildImpl()
    {
        var view = new FieldViewDto
        {
            Fields = Fields.Fields
        };

        return view;
    }

    public void Add(Expression<Func<TModel, object?>> selector, int? width = null)
    {
        Fields.Add(selector, width);
    }

    public IEnumerator GetEnumerator() => Fields.GetEnumerator();
}
