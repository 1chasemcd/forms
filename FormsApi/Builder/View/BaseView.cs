using System.Linq.Expressions;
using FormsApi.Definition.View;

namespace FormsApi.Builder.View;


public abstract class BaseView<TModel>
{
    public PropertyOrConstant<TModel, string?>? Title { get; set; }
    public int? Width { get; set; }
    public PropertyOrConstant<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstant<TModel, bool>? Visible { get; set; }
    internal BaseViewDto Build()
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
}
public abstract class BaseView<TModel, TThis> : BaseView<TModel>
    where TThis : BaseView<TModel>
{
    private TThis This => this as TThis ?? throw new Exception();
    public TThis WithTitle(string title)
    {
        base.Title = title;
        return This;
    }
    public TThis WithTitle(Expression<Func<TModel, string?>> title)
    {
        base.Title = title;
        return This;
    }
    public TThis WithWidth(int width)
    {
        base.Width = width;
        return This;
    }
    public TThis Disabled()
    {
        base.Enabled = false;
        return This;
    }
    public TThis EnabledWhen(Expression<Func<TModel, bool>> enabled)
    {
        base.Enabled = enabled;
        return This;
    }

    public TThis Hidden()
    {
        base.Visible = false;
        return This;
    }
    public TThis VisibleWhen(Expression<Func<TModel, bool>> visible)
    {
        base.Visible = visible;
        return This;
    }
}
