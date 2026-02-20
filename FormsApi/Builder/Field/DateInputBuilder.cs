using System;
using FormsApi.Form.Field;

namespace FormsApi.Builder.Field;

public class DateInputBuilder<TModel>(
    ModelMemberBuilder<TModel, DateOnly> propertyBuilder)
    : BaseFieldBuilder<TModel>
{
    public PropertyOrConstantBuilder<TModel, DateOnly>? MaxValue { get; set; }
    public PropertyOrConstantBuilder<TModel, DateOnly>? MinValue { get; set; }
    protected override DateInput BuildImpl()
    {
        return new DateInput()
        {
            Property = propertyBuilder.Build(),
            MaxValue = MaxValue?.Build(),
            MinValue = MinValue?.Build(),
        };
    }
}
