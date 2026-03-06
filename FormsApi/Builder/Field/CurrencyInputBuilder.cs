using System.Linq.Expressions;
using FormsApi.Common;
using FormsApi.Common.Types;
using FormsApi.Form.Field;

namespace FormsApi.Builder.Field;

public sealed class CurrencyInputBuilder<TModel>(
    ModelMemberBuilder<TModel, Currency?> propertyBuilder)
    : BaseInputBuilder<TModel, CurrencyInputBuilder<TModel>>
{
    public PropertyOrConstantBuilder<TModel, int>? MaxValue { get; set; }
    public PropertyOrConstantBuilder<TModel, int>? MinValue { get; set; }
    protected override CurrencyInput BuildInput()
    {
        return new CurrencyInput()
        {
            Property = propertyBuilder.Build(),
            MaxValue = MaxValue?.Build(),
            MinValue = MinValue?.Build()
        };
    }
    protected override string GetDefaultLabel() => propertyBuilder.Build().CamelCaseToWords();


    public CurrencyInputBuilder<TModel> WithMaxValue(int maxValue)
    {
        MaxValue = maxValue;
        return This;
    }
    public CurrencyInputBuilder<TModel> WithMaxValue(Expression<Func<TModel, int>> maxValueProperty)
    {
        MaxValue = maxValueProperty;
        return This;
    }

    public CurrencyInputBuilder<TModel> WithMinValue(int minValue)
    {
        MinValue = minValue;
        return This;
    }
    public CurrencyInputBuilder<TModel> WithMinValue(Expression<Func<TModel, int>> minValueProperty)
    {
        MinValue = minValueProperty;
        return This;
    }
}
