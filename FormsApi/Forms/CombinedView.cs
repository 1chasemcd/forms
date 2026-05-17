using System.Collections;
using System.Linq.Expressions;
using FormsApi.Common;
using FormsApi.Contract.View;

namespace FormsApi.Forms;

public sealed class CombinedView<TModel> : BaseView<TModel, CombinedView<TModel>>, IEnumerable
{
    public IList<BaseView<TModel>> Views { get; set; } = [];
    public bool IsUnified { get; set; }
    public CombinedView(PropertyOrConstant<TModel, string?>? title = null)
    {
        Title = title;
    }

    public CombinedView(Expression<Func<TModel, string?>> title)
    {
        Title = title;
    }

    protected override CombinedViewDto BuildImpl()
    {
        var view = new CombinedViewDto
        {
            Views = Views.Select(x => x.Build()),
            Unify = IsUnified || Title != null
        };

        return view;
    }
    public void Add(BaseView<TModel> view)
    {
        Views.Add(view);
    }
    IEnumerator IEnumerable.GetEnumerator() => Views.GetEnumerator();
    public CombinedView<TModel> Unify()
    {
        IsUnified = true;
        return this;
    }
}
