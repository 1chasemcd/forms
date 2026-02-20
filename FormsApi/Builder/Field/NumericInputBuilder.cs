
using System.Numerics;
using FormsApi.Form.Field;

namespace FormsApi.Builder.Field;

public class NumericInputBuilder<TModel, TInput>(
    ModelMemberBuilder<TModel, TInput> propertyBuilder)
    : BaseFieldBuilder<TModel> where TInput : INumber<TInput>
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
}
