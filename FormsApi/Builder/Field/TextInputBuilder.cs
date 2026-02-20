
using System.Linq.Expressions;
using FormsApi.Form.Field;

namespace FormsApi.Builder.Field;

public class TextInputBuilder<TModel>(
    ModelMemberBuilder<TModel, string> propertyBuilder)
    : BaseFieldBuilder<TModel, TextInputBuilder<TModel>>
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

    public TextInputBuilder<TModel> WithMaxLength(int maxLength)
    {
        MaxLength = maxLength;
        return This;
    }
    public TextInputBuilder<TModel> WithMaxLength(Expression<Func<TModel, int>> maxLength)
    {
        MaxLength = maxLength;
        return This;
    }
}
