
using FormsApi.Form.Field;

namespace FormsApi.Builder.Field;

public class TextInputBuilder<TModel>(
    ModelMemberBuilder<TModel, string> propertyBuilder)
    : BaseFieldBuilder<TModel>
{
    public PropertyOrConstantBuilder<TModel, int>? MaxLength { get; set; }
    protected override TextInput BuildImpl()
    {
        return new TextInput()
        {
            Property = propertyBuilder.Build(),
            MaxLength = MaxLength?.Build()
        };
    }
}
