using System.Diagnostics;
using System.Linq.Expressions;
using FormsApi.Common;
using FormsApi.Contract.View;

namespace FormsApi.Forms;


public interface IFormView<TModel>
{
    PropertyOrConstant<TModel, string?>? Title { get; set; }
    int? Width { get; set; }
    PropertyOrConstant<TModel, bool>? Enabled { get; set; }
    PropertyOrConstant<TModel, bool>? Visible { get; set; }
    BaseViewDto Build();
}

public abstract class FormView<TModel, TThis> : IFormView<TModel>
    where TThis : class, IFormView<TModel>
{
    public PropertyOrConstant<TModel, string?>? Title { get; set; }
    public int? Width { get; set; }
    public PropertyOrConstant<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstant<TModel, bool>? Visible { get; set; }
    public BaseViewDto Build()
    {
        BaseViewDto baseView = BuildImpl();
        return baseView with
        {
            Title = Title?.Build(),
            Width = Width,
            Enabled = Enabled?.Build(),
            Visible = Visible?.Build()
        };
    }

    protected abstract BaseViewDto BuildImpl();
    private TThis This => this as TThis ?? throw new UnreachableException(
        $"{GetType().Name} must inherit FormView<TModel, TThis> correctly.");
    public TThis WithTitle(string title)
    {
        Title = title;
        return This;
    }
    public TThis WithTitle(Expression<Func<TModel, string?>> title)
    {
        Title = title;
        return This;
    }
    public TThis WithWidth(int width)
    {
        Width = width;
        return This;
    }
    public TThis Disabled()
    {
        Enabled = false;
        return This;
    }
    public TThis EnabledWhen(Expression<Func<TModel, bool>> enabled)
    {
        Enabled = enabled;
        return This;
    }

    public TThis Hidden()
    {
        Visible = false;
        return This;
    }
    public TThis VisibleWhen(Expression<Func<TModel, bool>> visible)
    {
        Visible = visible;
        return This;
    }
}
