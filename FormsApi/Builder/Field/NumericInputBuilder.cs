
using System.Linq.Expressions;
using System.Numerics;
using FormsApi.Form.Field;

namespace FormsApi.Builder.Field;

public sealed class NumericInputBuilder<TModel, TInput>(
    ModelMemberBuilder<TModel, TInput> propertyBuilder)
    : BaseFieldBuilder<TModel, NumericInputBuilder<TModel, TInput>>
    where TInput : INumber<TInput>
{
    public PropertyOrConstantBuilder<TModel, int>? MaxValue { get; set; }
    public PropertyOrConstantBuilder<TModel, int>? MinValue { get; set; }
    public PropertyOrConstantBuilder<TModel, int>? Precision { get; set; }
    public PropertyOrConstantBuilder<TModel, int>? Scale { get; set; }
    protected override NumericInput BuildImpl()
    {
        return new NumericInput()
        {
            Property = propertyBuilder.Build(),
            MaxValue = MaxValue?.Build(),
            MinValue = MinValue?.Build(),
            Precision = Precision?.Build(),
            Scale = Scale?.Build()
        };
    }

    public NumericInputBuilder<TModel, TInput> WithMaxValue(int maxValue)
    {
        MaxValue = maxValue;
        return This;
    }
    public NumericInputBuilder<TModel, TInput> WithMaxValue(Expression<Func<TModel, int>> maxValueProperty)
    {
        MaxValue = maxValueProperty;
        return This;
    }

    public NumericInputBuilder<TModel, TInput> WithMinValue(int minValue)
    {
        MinValue = minValue;
        return This;
    }
    public NumericInputBuilder<TModel, TInput> WithMinValue(Expression<Func<TModel, int>> minValueProperty)
    {
        MinValue = minValueProperty;
        return This;
    }

    public NumericInputBuilder<TModel, TInput> WithPrecision(int precision)
    {
        Precision = precision;
        return This;
    }
    public NumericInputBuilder<TModel, TInput> WithPrecision(Expression<Func<TModel, int>> precision)
    {
        Precision = precision;
        return This;
    }

    public NumericInputBuilder<TModel, TInput> WithScale(int scale)
    {
        Scale = scale;
        return This;
    }
    public NumericInputBuilder<TModel, TInput> WithScale(Expression<Func<TModel, int>> scale)
    {
        Scale = scale;
        return This;
    }
}
