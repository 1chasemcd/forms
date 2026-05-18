using System.Collections;
using System.Linq.Expressions;
using FormsApi.Common;
using FormsApi.Contract.View;

namespace FormsApi.Forms;

public sealed class ControlView<TModel> : View<TModel, ControlView<TModel>>, IEnumerable
{
    public ControlList<TModel> Fields { get; set; } = [];

    public ControlView(PropertyOrConstant<TModel, string?>? title = null)
    {
        Title = title;
    }

    public ControlView(Expression<Func<TModel, string?>> title)
    {
        Title = title;
    }

    protected override FieldViewDto BuildImpl()
    {
        var view = new FieldViewDto
        {
            Fields = Fields.Controls
        };

        return view;
    }

    public void Add(Expression<Func<TModel, object?>> selector, int? width = null)
    {
        Fields.Add(selector, width);
    }

    public IEnumerator GetEnumerator() => (Fields as IEnumerable).GetEnumerator();
}
