using System.Linq.Expressions;
using FormsApi.Common.Types;
using FormsApi.Definition.Field;
using FormsApi.Recalculate;

namespace FormsApi.Builder.Field;

public sealed class CurrencyInputBuilder<TModel>
    : BaseFieldBuilder<TModel, Currency?>, IEnablable<TModel>, IRequirable<TModel>, IValueRangable<TModel, Currency?>, IRecalculatable<TModel>
{
    public override FieldType Type => FieldType.Currency;

    public PropertyOrConstantBuilder<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Required { get; set; }
    public PropertyOrConstantBuilder<TModel, Currency?>? MaxValue { get; set; }
    public PropertyOrConstantBuilder<TModel, Currency?>? MinValue { get; set; }
    public IRecalculateEventBuilder<TModel>? RecalculateEvent { get; private set; }
    public void AddRecalc<TService>(Expression<Func<TService, Func<TModel, PostRecalculateEvent?>>> method)
    {
        RecalculateEvent = new RecalculateEventBuilder<TModel, TService>(method);
    }
}