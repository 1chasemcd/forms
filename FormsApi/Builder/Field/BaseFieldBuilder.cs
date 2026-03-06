
using System.Linq.Expressions;
using FormsApi.Form.Field;
using FormsApi.Form.Primitives;

namespace FormsApi.Builder.Field;

public abstract class BaseFieldBuilder<TModel>
{
    public PropertyOrConstantBuilder<TModel, string>? Label { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Hidden { get; set; }
    public FormElementSize? Width { get; set; }

    internal virtual BaseField Build()
    {
        BaseField field = BuildField();

        return field with
        {
            Label = Label?.Build() ?? new Constant(GetDefaultLabel()),
            Hidden = Hidden?.Build(),
            Width = Width
        };
    }
    protected abstract BaseField BuildField();
    protected abstract string GetDefaultLabel();
}

public abstract class BaseFieldBuilder<TModel, TThis> : BaseFieldBuilder<TModel>
    where TThis : BaseFieldBuilder<TModel, TThis>
{
    internal TThis This => (TThis)this;
    public TThis WithLabel(string label)
    {
        Label = label;
        return This;
    }
    public TThis WithLabel(Expression<Func<TModel, string>> labelProperty)
    {
        Label = labelProperty;
        return This;
    }
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
