using System.Linq.Expressions;
using FormsApi.Common;
using FormsApi.Form.Field;

namespace FormsApi.Builder.Field;

public sealed class DateInputBuilder<TModel>(
    ModelMemberBuilder<TModel, DateOnly?> propertyBuilder)
    : BaseInputBuilder<TModel, DateInputBuilder<TModel>>
{
    public PropertyOrConstantBuilder<TModel, DateOnly>? MaxValue { get; set; }
    public PropertyOrConstantBuilder<TModel, DateOnly>? MinValue { get; set; }
    protected override DateInput BuildInput()
    {
        return new DateInput()
        {
            Property = propertyBuilder.Build(),
            MaxValue = MaxValue?.Build(),
            MinValue = MinValue?.Build(),
        };
    }

    protected override string GetDefaultLabel() => propertyBuilder.Build().CamelCaseToWords();

    public DateInputBuilder<TModel> WithMaxValue(DateOnly maxValue)
    {
        MaxValue = maxValue;
        return This;
    }
    public DateInputBuilder<TModel> WithMaxValue(Expression<Func<TModel, DateOnly>> maxValueProperty)
    {
        MaxValue = maxValueProperty;
        return This;
    }

    public DateInputBuilder<TModel> WithMinValue(DateOnly minValue)
    {
        MinValue = minValue;
        return This;
    }
    public DateInputBuilder<TModel> WithMinValue(Expression<Func<TModel, DateOnly>> minValueProperty)
    {
        MinValue = minValueProperty;
        return This;
    }
}
