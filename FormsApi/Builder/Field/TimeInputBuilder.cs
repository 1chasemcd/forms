using System.Linq.Expressions;
using FormsApi.Common;
using FormsApi.Form.Field;

namespace FormsApi.Builder.Field;

public sealed class TimeInputBuilder<TModel>(
    ModelMemberBuilder<TModel, TimeOnly?> propertyBuilder)
    : BaseInputBuilder<TModel, TimeInputBuilder<TModel>>
{
    public PropertyOrConstantBuilder<TModel, TimeOnly>? MaxValue { get; set; }
    public PropertyOrConstantBuilder<TModel, TimeOnly>? MinValue { get; set; }
    protected override TimeInput BuildInput()
    {
        return new TimeInput()
        {
            Property = propertyBuilder.Build(),
            MaxValue = MaxValue?.Build(),
            MinValue = MinValue?.Build(),
        };
    }

    protected override string GetDefaultLabel() => propertyBuilder.Build().CamelCaseToWords();

    public TimeInputBuilder<TModel> WithMaxValue(TimeOnly maxValue)
    {
        MaxValue = maxValue;
        return This;
    }
    public TimeInputBuilder<TModel> WithMaxValue(Expression<Func<TModel, TimeOnly>> maxValueProperty)
    {
        MaxValue = maxValueProperty;
        return This;
    }

    public TimeInputBuilder<TModel> WithMinValue(TimeOnly minValue)
    {
        MinValue = minValue;
        return This;
    }
    public TimeInputBuilder<TModel> WithMinValue(Expression<Func<TModel, TimeOnly>> minValueProperty)
    {
        MinValue = minValueProperty;
        return This;
    }
}
