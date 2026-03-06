
using System.Linq.Expressions;
using FormsApi.Common;
using FormsApi.Form.Field;

namespace FormsApi.Builder.Field;

public sealed class TextInputBuilder<TModel>(
    ModelMemberBuilder<TModel, string?> propertyBuilder)
    : BaseInputBuilder<TModel, TextInputBuilder<TModel>>
{
    public PropertyOrConstantBuilder<TModel, int>? MaxLength { get; set; }
    protected override TextInput BuildInput()
    {
        return new TextInput()
        {
            Property = propertyBuilder.Build(),
            MaxLength = MaxLength?.Build()
        };
    }

    protected override string GetDefaultLabel() => propertyBuilder.Build().CamelCaseToWords();

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
