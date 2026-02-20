using FormsApi.Form.Field;

namespace FormsApi.Builder.Field;

public sealed class CheckBoxInputBuilder<TModel>(
    ModelMemberBuilder<TModel, bool> propertyBuilder)
    : BaseFieldBuilder<TModel, CheckBoxInputBuilder<TModel>>
{
    protected override CheckBoxInput BuildImpl()
    {
        return new CheckBoxInput()
        {
            Property = propertyBuilder.Build(),
        };
    }
}
