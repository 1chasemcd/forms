using System.Linq.Expressions;
using FormsApi.Common;
using FormsApi.Form.Field;
using FormsApi.Form.Primitives;
using FormsApi.Recalculate;

namespace FormsApi.Builder.Field;

public abstract class BaseInputBuilder<TModel, TThis> : BaseFieldBuilder<TModel, TThis>
    where TThis : BaseInputBuilder<TModel, TThis>
{
    public IEnumerable<ModelMemberBuilder<TModel, object>>? PropsToUpdate { get; set; }
    public IFormActionBuilder<TModel>? Action { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Required { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Disabled { get; set; }

    protected override BaseField BuildField()
    {
        BaseInput input = BuildInput();
        return input with
        {
            Label = new Constant(input.Property.CamelCaseToWords()),
            RecalculateEvent = BuildRecalculateEvent(),
            Required = Required?.Build(),
            Disabled = Disabled?.Build(),
        };
    }

    private RecalculateEvent? BuildRecalculateEvent()
    {
        if (PropsToUpdate is null && Action is null)
            return null;
        return new RecalculateEvent()
        {
            PropertiesToUpdate = PropsToUpdate?.Select(p => p.Build()),
            FormAction = Action?.Build()
        };
    }

    protected abstract BaseInput BuildInput();

    public TThis WithPropsToUpdate(params Expression<Func<TModel, object>>[] props)
    {
        PropsToUpdate = props.Select(x => new ModelMemberBuilder<TModel, object>(x));
        return This;
    }

    public TThis WithActionOnChange<TService>(Expression<Func<TService, Func<TModel, RecalculateEventResult>>> serviceMethod)
    {
        Action = new FormActionBuilder<TService, TModel>(serviceMethod);
        return This;
    }

    public TThis WithActionOnChange<TService>(Expression<Func<TService, Func<RecalculateEventResult>>> serviceMethod)
    {
        Action = new FormActionBuilder<TService, TModel>(serviceMethod);
        return This;
    }

    public TThis WithRequired(bool required = true)
    {
        Required = required;
        return This;
    }
    public TThis WithRequired(Expression<Func<TModel, bool>> requiredProperty)
    {
        Required = requiredProperty;
        return This;
    }

    public TThis WithDisabled(bool disabled = true)
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
