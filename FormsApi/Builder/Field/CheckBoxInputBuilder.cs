using FormsApi.Common;
using FormsApi.Form.Field;

namespace FormsApi.Builder.Field;

public sealed class CheckBoxInputBuilder<TModel>(
    ModelMemberBuilder<TModel, bool?> propertyBuilder)
    : BaseInputBuilder<TModel, CheckBoxInputBuilder<TModel>>
{
    protected override CheckBoxInput BuildInput()
    {
        return new CheckBoxInput()
        {
            Property = propertyBuilder.Build(),
        };
    }
    protected override string GetDefaultLabel() => propertyBuilder.Build().CamelCaseToWords();
}
