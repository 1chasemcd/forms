using System.Linq.Expressions;
using FormsApi.Definition.Field;
using FormsApi.Recalculate;

namespace FormsApi.Builder.Field;

public sealed class DateInputBuilder<TModel>
: BaseFieldBuilder<TModel, DateOnly?>, IEnablable<TModel>, IRequirable<TModel>, IValueRangable<TModel, DateOnly?>, IRecalculatable<TModel>
{
    public override FieldType Type => FieldType.Date;

    public PropertyOrConstantBuilder<TModel, bool?>? Enabled { get; set; }
    public PropertyOrConstantBuilder<TModel, bool?>? Required { get; set; }
    public PropertyOrConstantBuilder<TModel, DateOnly?>? MaxValue { get; set; }
    public PropertyOrConstantBuilder<TModel, DateOnly?>? MinValue { get; set; }
    public IRecalculateEventBuilder<TModel>? RecalculateEvent { get; private set; }
    public void AddRecalc<TService>(Expression<Func<TService, Func<TModel, PostRecalculateEvent?>>> method)
    {
        RecalculateEvent = new RecalculateEventBuilder<TModel, TService>(method);
    }
}