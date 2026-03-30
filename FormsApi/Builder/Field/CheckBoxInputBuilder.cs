using System.Linq.Expressions;
using FormsApi.Definition.Field;
using FormsApi.Recalculate;

namespace FormsApi.Builder.Field;

public sealed class CheckBoxInputBuilder<TModel>
    : BaseFieldBuilder<TModel, bool?>, IEnablable<TModel>, IRequirable<TModel>, IRecalculatable<TModel>
{
    public override FieldType Type => FieldType.CheckBox;

    public PropertyOrConstantBuilder<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Required { get; set; }
    public IRecalculateEventBuilder<TModel>? RecalculateEvent { get; private set; }
    public void AddRecalc<TService>(Expression<Func<TService, Func<TModel, PostRecalculateEvent?>>> method)
    {
        RecalculateEvent = new RecalculateEventBuilder<TModel, TService>(method);
    }
}
