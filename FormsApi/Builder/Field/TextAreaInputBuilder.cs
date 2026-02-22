using System.Linq.Expressions;
using FormsApi.Form.Field;

namespace FormsApi.Builder.Field;

public sealed class TextAreaInputBuilder<TModel>(
    ModelMemberBuilder<TModel, IEnumerable<string>?> propertyBuilder)
    : BaseFieldBuilder<TModel, TextAreaInputBuilder<TModel>>
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

    public TextAreaInputBuilder<TModel> WithMaxLength(int maxLength)
    {
        MaxLength = maxLength;
        return This;
    }
    public TextAreaInputBuilder<TModel> WithMaxLength(Expression<Func<TModel, int>> maxLength)
    {
        MaxLength = maxLength;
        return This;
    }
}
