using System.Linq.Expressions;
using FormsApi.Common.Types;
using FormsApi.Definition.Field;
using FormsApi.Recalculate;

namespace FormsApi.Builder.Field;

public sealed class ButtonFieldBuilder<TModel>
    : BaseFieldBuilder<TModel, Button?>, IRecalculatable<TModel>, IEnablable<TModel>
{
    public PropertyOrConstantBuilder<TModel, bool>? Enabled { get; set; }
    public override FieldType Type => FieldType.Button;
    public IRecalculateEventBuilder<TModel>? RecalculateEvent { get; private set; }
    public void AddRecalc<TService>(Expression<Func<TService, Func<TModel, PostRecalculateEvent?>>> method)
    {
        RecalculateEvent = new RecalculateEventBuilder<TModel, TService>(method);
    }
}
