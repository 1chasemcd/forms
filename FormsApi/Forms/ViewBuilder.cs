using System.Diagnostics;
using System.Linq.Expressions;
using FormsApi.Common;

namespace FormsApi.Forms;


public interface IViewBuilder<TModel>
{
    FormValueRefBuilder<TModel, string>? Title { get; }
    int? Width { get; set; }
    FormValueRefBuilder<TModel, bool>? Visible { get; }
}

public abstract class ViewBuilder<TModel, TThis> : IViewBuilder<TModel>
    where TThis : class, IViewBuilder<TModel>
{
    public FormValueRefBuilder<TModel, string>? Title { get; protected set; }
    public int? Width { get; set; }
    public FormValueRefBuilder<TModel, bool>? Visible { get; protected set; }
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
