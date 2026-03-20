using System.Linq.Expressions;
using FormsApi.Common;
using FormsApi.Form.Field;
using FormsApi.Form.Primitives;
using FormsApi.Recalculate;

namespace FormsApi.Builder.Field;

public static class Button
{
    public static ButtonFieldBuilder<TModel> OnModel<TModel>(TModel model)
    {
        TModel? _ = model; // just to remove unused parameter warning
        return new ButtonFieldBuilder<TModel>();
    }

    public static ButtonFieldBuilder<TModel> OnModel<TModel>() => new();

}

public sealed class ButtonFieldBuilder<TModel>
    : BaseFieldBuilder<TModel, ButtonFieldBuilder<TModel>>
{
    public IRecalculateEventBuilder<TModel>? Recalculate { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Disabled { get; set; }
    protected override ButtonField BuildField()
    {
        if (Recalculate == null) throw new NullReferenceException();
        RecalculateEvent recalculate = Recalculate.Build();
        return new ButtonField()
        {
            Label = new Constant(recalculate.Method.CamelCaseToWords()),
            RecalculateEvent = recalculate
        };
    }

    public ButtonFieldBuilder<TModel> WithRecalculate<TService>(Expression<Func<TService, Func<TModel, RecalculateEventResult<TModel>>>> serviceMethod)
    {
        Recalculate = new RecalculateEventBuilder<TModel, TService, TModel>(serviceMethod);
        return This;
    }

    public ButtonFieldBuilder<TModel> WithRecalculate<TService>(Expression<Func<TService, Func<RecalculateEventResult<TModel>>>> serviceMethod)
    {
        Recalculate = new RecalculateEventBuilder<TModel, TService, TModel>(serviceMethod);
        return This;
    }

    public ButtonFieldBuilder<TModel> WithRecalculate<TService, TMethod>(Expression<Func<TService, Func<TMethod, RecalculateEventResult<TMethod>>>> serviceMethod)
    {
        Recalculate = new RecalculateEventBuilder<TModel, TService, TMethod>(serviceMethod);
        return This;
    }

    public ButtonFieldBuilder<TModel> WithRecalculate<TService, TMethod>(Expression<Func<TService, Func<RecalculateEventResult<TMethod>>>> serviceMethod)
    {
        Recalculate = new RecalculateEventBuilder<TModel, TService, TMethod>(serviceMethod);
        return This;
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
