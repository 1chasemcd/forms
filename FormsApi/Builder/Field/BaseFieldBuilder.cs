
using System.Linq.Expressions;
using FormsApi.Form.Field;
using FormsApi.Form.Primitives;

namespace FormsApi.Builder.Field;

public abstract class BaseFieldBuilder<TModel>
{
    public PropertyOrConstantBuilder<TModel, string>? Label { get; set; }
    public IEnumerable<ModelMemberBuilder<TModel, object>>? PropsToUpdate { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Hidden { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Disabled { get; set; }
    public FormElementSize? Width { get; set; }

    internal BaseField Build()
    {
        BaseField field = BuildImpl();

        return field with
        {
            Label = Label?.Build(),
            PropertiesToUpdateOnChange = PropsToUpdate?.Select(x => x.Build()),
            Hidden = Hidden?.Build(),
            Disabled = Disabled?.Build(),
            Width = Width
        };
    }
    protected abstract BaseField BuildImpl();
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
    public TThis WithPropsToUpdate(params Expression<Func<TModel, object>>[] props)
    {
        PropsToUpdate = props.Select(x => new ModelMemberBuilder<TModel, object>(x));
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
    public TThis WithDisabled(bool disabled)
    {
        Disabled = disabled;
        return This;
    }
    public TThis WithDisabled(Expression<Func<TModel, bool>> disabledProperty)
    {
        Disabled = disabledProperty;
        return This;
    }
    public TThis WithWidth(FormElementSize width)
    {
        Width = width;
        return This;
    }
}
