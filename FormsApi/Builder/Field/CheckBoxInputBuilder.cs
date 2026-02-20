using FormsApi.Form.Field;

namespace FormsApi.Builder.Field;

public class CheckBoxInputBuilder<TModel>(
    ModelMemberBuilder<TModel, bool> propertyBuilder)
    : BaseFieldBuilder<TModel>
{
    protected override CheckBoxInput BuildImpl()
    {
        return new CheckBoxInput()
        {
            Property = propertyBuilder.Build(),
        };
    }
}
