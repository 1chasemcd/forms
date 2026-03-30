using System.Linq.Expressions;
using FormsApi.Common.Types;
using FormsApi.Definition.Field;

namespace FormsApi.Builder.Field;

public sealed class LabelValueBuilder<TModel>
    : BaseFieldBuilder<TModel, LabelValue>
{
    public override FieldType Type => FieldType.LabelValue;
}