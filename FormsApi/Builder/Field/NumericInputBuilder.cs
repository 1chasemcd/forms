
using System.Linq.Expressions;
using System.Numerics;
using FormsApi.Definition.Field;
using FormsApi.Recalculate;

namespace FormsApi.Builder.Field;

public sealed class NumericInputBuilder<TModel, TInput>
: BaseFieldBuilder<TModel, TInput?>, IEnablable<TModel>, IRequirable<TModel>, IValueRangable<TModel, TInput>, IPrecisionAndScalable<TModel>, IRecalculatable<TModel>
  where TInput : INumber<TInput>
{
    public override FieldType Type => FieldType.Numeric;

    public PropertyOrConstantBuilder<TModel, bool?>? Enabled { get; set; }
    public PropertyOrConstantBuilder<TModel, bool?>? Required { get; set; }
    public PropertyOrConstantBuilder<TModel, TInput?>? MaxValue { get; set; }
    public PropertyOrConstantBuilder<TModel, TInput?>? MinValue { get; set; }
    public PropertyOrConstantBuilder<TModel, int?>? Precision { get; set; }
    public PropertyOrConstantBuilder<TModel, int?>? Scale { get; set; }
    public IRecalculateEventBuilder<TModel>? RecalculateEvent { get; private set; }
    public void AddRecalc<TService>(Expression<Func<TService, Func<TModel, PostRecalculateEvent?>>> method)
    {
        RecalculateEvent = new RecalculateEventBuilder<TModel, TService>(method);
    }
}