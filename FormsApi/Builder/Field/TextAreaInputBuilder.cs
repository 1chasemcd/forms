using FormsApi.Form.Field;

namespace FormsApi.Builder.Field;

public class TextAreaInputBuilder<TModel>(
    ModelMemberBuilder<TModel, IEnumerable<string>> propertyBuilder)
    : BaseFieldBuilder<TModel>
{
    public PropertyOrConstantBuilder<TModel, int>? MaxLength { get; set; }
    protected override TextAreaInput BuildImpl()
    {
        return new TextAreaInput()
        {
            Property = propertyBuilder.Build(),
            MaxLength = MaxLength?.Build()
        };
    }
}
