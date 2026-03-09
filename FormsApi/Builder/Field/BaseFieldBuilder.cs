
using System.Linq.Expressions;
using FormsApi.Form.Field;
using FormsApi.Form.Primitives;

namespace FormsApi.Builder.Field;



public abstract class BaseFieldBuilder<TModel>
{
    public PropertyOrConstantBuilder<TModel, bool>? Hidden { get; set; }
    public FormElementSize? Width { get; set; }

    internal virtual BaseField Build()
    {
        BaseField field = BuildField();

        return field with
        {
            Hidden = Hidden?.Build(),
            Width = Width
        };
    }
    protected abstract BaseField BuildField();
}

public abstract class BaseFieldBuilder<TModel, TThis> : BaseFieldBuilder<TModel>
    where TThis : BaseFieldBuilder<TModel, TThis>
{
    internal TThis This => (TThis)this;
    public TThis WithHidden(bool hidden)
    {
        Hidden = hidden;
        return This;
    }
    public TThis WithHidden(Expression<Func<TModel, bool>> hiddenProperty)
    {
        Hidden = hiddenProperty;
        return This;
    }
    public TThis WithWidth(FormElementSize width)
    {
        Width = width;
        return This;
    }
}
