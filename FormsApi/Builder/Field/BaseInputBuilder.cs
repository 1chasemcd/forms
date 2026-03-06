using System;
using System.Linq.Expressions;
using FormsApi.Form.Field;
using FormsApi.Form.Primitives;

namespace FormsApi.Builder.Field;

public abstract class BaseInputBuilder<TModel> : BaseFieldBuilder<TModel>
{
    public IEnumerable<ModelMemberBuilder<TModel, object>>? PropsToUpdate { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Disabled { get; set; }

    protected override BaseField BuildField()
    {
        BaseInput input = BuildInput();
        return input with
        {
            PropertiesToUpdateOnChange = PropsToUpdate?.Select(x => x.Build()),
            Disabled = Disabled?.Build(),
        };
    }

    protected abstract BaseInput BuildInput();
}

public abstract class BaseInputBuilder<TModel, TThis> : BaseInputBuilder<TModel>
    where TThis : BaseInputBuilder<TModel, TThis>
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

    public TThis WithPropsToUpdate(params Expression<Func<TModel, object>>[] props)
    {
        PropsToUpdate = props.Select(x => new ModelMemberBuilder<TModel, object>(x));
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
}
