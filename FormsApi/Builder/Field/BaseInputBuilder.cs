using System.Linq.Expressions;
using FormsApi.Common;
using FormsApi.Form.Field;
using FormsApi.Form.Primitives;
using FormsApi.Recalculate;

namespace FormsApi.Builder.Field;

public abstract class BaseInputBuilder<TModel, TThis> : BaseFieldBuilder<TModel, TThis>
    where TThis : BaseInputBuilder<TModel, TThis>
{
    public IRecalculateEventBuilder<TModel>? Recalculate { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Required { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Disabled { get; set; }

    protected override BaseField BuildField()
    {
        BaseInput input = BuildInput();
        return input with
        {
            Label = new Constant(input.Property.CamelCaseToWords()),
            RecalculateEvent = Recalculate?.Build(),
            Required = Required?.Build(),
            Disabled = Disabled?.Build(),
        };
    }

    protected abstract BaseInput BuildInput();

    public TThis WithRecalculate<TService>(Expression<Func<TService, Func<TModel, PostRecalculateEvent?>>> serviceMethod)
    {
        Recalculate = new RecalculateEventBuilder<TModel, TService>(serviceMethod);
        return This;
    }

    public TThis WithRecalculate<TService>(Expression<Func<TService, Func<PostRecalculateEvent?>>> serviceMethod)
    {
        Recalculate = new RecalculateEventBuilder<TModel, TService>(serviceMethod);
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
