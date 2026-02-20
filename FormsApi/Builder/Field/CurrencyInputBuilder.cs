using FormsApi.Common.Types;
using FormsApi.Form.Field;

namespace FormsApi.Builder.Field;

public class CurrencyInputBuilder<TModel>(
    ModelMemberBuilder<TModel, Currency> propertyBuilder)
    : BaseFieldBuilder<TModel>
{
    public PropertyOrConstantBuilder<TModel, int>? MaxValue { get; set; }
    public PropertyOrConstantBuilder<TModel, int>? MinValue { get; set; }
    protected override CurrencyInput BuildImpl()
    {
        return new CurrencyInput()
        {
            Property = propertyBuilder.Build(),
            MaxValue = MaxValue?.Build(),
            MinValue = MinValue?.Build()
        };
    }
}
