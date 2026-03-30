

using System.Linq.Expressions;
using FormsApi.Definition.Field;
using FormsApi.Recalculate;

namespace FormsApi.Builder.Field;

public sealed class TextInputBuilder<TModel>
    : BaseFieldBuilder<TModel, string?>, IMaxLengthable<TModel>, IRequirable<TModel>, IEnablable<TModel>, IRecalculatable<TModel>
{
    public PropertyOrConstantBuilder<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Required { get; set; }
    public PropertyOrConstantBuilder<TModel, int>? MaxLength { get; set; }
    public override FieldType Type => FieldType.Text;
    public IRecalculateEventBuilder<TModel>? RecalculateEvent { get; private set; }
    public void AddRecalc<TService>(Expression<Func<TService, Func<TModel, PostRecalculateEvent?>>> method)
    {
        RecalculateEvent = new RecalculateEventBuilder<TModel, TService>(method);
    }
}
