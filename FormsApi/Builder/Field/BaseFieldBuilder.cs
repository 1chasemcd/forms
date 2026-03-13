
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

        field = field with
        {
            Hidden = Hidden?.Build(),
            Width = Width
        };

        if (Label is not null)
            field = field with { Label = Label.Build() };

        return field;
    }
    protected abstract BaseField BuildField();
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
    public TThis WithHidden(bool hidden = true)
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
