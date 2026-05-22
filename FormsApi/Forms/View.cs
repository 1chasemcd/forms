using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using FormsApi.Common;
using FormsApi.Contract;
using FormsApi.Contract.View;

namespace FormsApi.Forms;


public interface IView<TModel>
{
    PropertyOrConstantDto? Title { get; }
    int? Width { get; set; }
    PropertyOrConstantDto? Visible { get; }
}

public abstract class View<TModel, TThis> : IView<TModel>
    where TThis : class, IView<TModel>
{
    public PropertyOrConstantDto? Title { get; protected set; }
    public int? Width { get; set; }
    public PropertyOrConstantDto? Visible { get; protected set; }
    private TThis This => this as TThis ?? throw new UnreachableException(
        $"{GetType().Name} must inherit FormView<TModel, TThis> correctly.");
    public TThis WithTitle(string title)
    {
        Title = new ConstantDto(title);
        return This;
    }
    public TThis WithTitle(Expression<Func<TModel, string?>> title)
    {
        Title = new PropertyDto(title.GetPropertyName());
        return This;
    }
    public TThis WithWidth(int width)
    {
        Width = width;
        return This;
    }

    public TThis Hidden()
    {
        Visible = new ConstantDto(false);
        return This;
    }
    public TThis VisibleWhen(Expression<Func<TModel, bool>> visible)
    {
        Visible = new PropertyDto(visible.GetPropertyName());
        return This;
    }
}
