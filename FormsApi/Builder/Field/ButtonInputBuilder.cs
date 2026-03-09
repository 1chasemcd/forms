using FormsApi.Common.Types;
using FormsApi.Form.Field;

namespace FormsApi.Builder.Field;

public sealed class ButtonInputBuilder<TModel>(
    ModelMemberBuilder<TModel, Button?> propertyBuilder)
    : BaseInputBuilder<TModel, ButtonInputBuilder<TModel>>
{
    protected override ButtonInput BuildInput()
    {
        return new ButtonInput()
        {
            Property = propertyBuilder.Build()
        };
    }
}
