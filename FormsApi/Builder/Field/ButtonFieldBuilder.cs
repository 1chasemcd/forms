using System.Linq.Expressions;
using FormsApi.Common;
using FormsApi.Form.Field;
using FormsApi.Form.Primitives;
using FormsApi.Recalculate;

namespace FormsApi.Builder.Field;

public static class Button
{
    public static ButtonFieldBuilder<TModel> Build<TModel>(TModel model)
    {
        TModel? _ = model; // just to remove unused parameter warning
        return new ButtonFieldBuilder<TModel>();
    }

    public static ButtonFieldBuilder<TModel> Build<TModel>() => new();

}

public sealed class ButtonFieldBuilder<TModel>
    : BaseFieldBuilder<TModel, ButtonFieldBuilder<TModel>>
{
    public IEnumerable<ModelMemberBuilder<TModel, object>>? PropsToUpdate { get; set; }
    public IFormActionBuilder<TModel>? FormActionBuilder { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Disabled { get; set; }
    protected override ButtonField BuildField()
    {
        RecalculateEvent onChange = BuildRecalculateEvent();
        return new ButtonField()
        {
            Label = new Constant(onChange.FormAction?.Method.CamelCaseToWords() ?? "Button"),
            RecalculateEvent = onChange
        };
    }

    private RecalculateEvent BuildRecalculateEvent()
    {
        return new RecalculateEvent()
        {
            PropertiesToUpdate = PropsToUpdate?.Select(p => p.Build()),
            FormAction = FormActionBuilder?.Build()
        };
    }
    public ButtonFieldBuilder<TModel> WithPropsToUpdate(params Expression<Func<TModel, object>>[] props)
    {
        PropsToUpdate = props.Select(x => new ModelMemberBuilder<TModel, object>(x));
        return this;
    }

    public ButtonFieldBuilder<TModel> WithActionOnChange<TService>(Expression<Func<TService, Func<TModel, RecalculateEventResult>>> serviceMethod)
    {
        FormActionBuilder = new FormActionBuilder<TService, TModel>(serviceMethod);
        return this;
    }

    public ButtonFieldBuilder<TModel> WithActionOnChange<TService>(Expression<Func<TService, Func<RecalculateEventResult>>> serviceMethod)
    {
        FormActionBuilder = new FormActionBuilder<TService, TModel>(serviceMethod);
        return this;
    }

    public ButtonFieldBuilder<TModel> WithDisabled(bool disabled = true)
    {
        Disabled = disabled;
        return This;
    }
    public ButtonFieldBuilder<TModel> WithDisabled(Expression<Func<TModel, bool>> disabledProperty)
    {
        Disabled = disabledProperty;
        return This;
    }
}
