using System.Linq.Expressions;
using FormsApi.Definition.Field;
using FormsApi.Recalculate;

namespace FormsApi.Builder.Field;

public sealed class TimeInputBuilder<TModel>
: BaseFieldBuilder<TModel, TimeOnly?>, IEnablable<TModel>, IRequirable<TModel>, IValueRangable<TModel, TimeOnly?>, IRecalculatable<TModel>
{
    public override FieldType Type => FieldType.Time;
    public PropertyOrConstantBuilder<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Required { get; set; }
    public PropertyOrConstantBuilder<TModel, TimeOnly?>? MaxValue { get; set; }
    public PropertyOrConstantBuilder<TModel, TimeOnly?>? MinValue { get; set; }
    public IRecalculateEventBuilder<TModel>? RecalculateEvent { get; private set; }
    public void AddRecalc<TService>(Expression<Func<TService, Func<TModel, PostRecalculateEvent?>>> method)
    {
        RecalculateEvent = new RecalculateEventBuilder<TModel, TService>(method);
    }
}