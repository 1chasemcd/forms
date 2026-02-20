using System;
using FormsApi.Form.Field;

namespace FormsApi.Builder.Field;

public class TimeInputBuilder<TModel>(
    ModelMemberBuilder<TModel, TimeOnly> propertyBuilder)
    : BaseFieldBuilder<TModel>
{
    public PropertyOrConstantBuilder<TModel, TimeOnly>? MaxValue { get; set; }
    public PropertyOrConstantBuilder<TModel, TimeOnly>? MinValue { get; set; }
    protected override TimeInput BuildImpl()
    {
        return new TimeInput()
        {
            Property = propertyBuilder.Build(),
            MaxValue = MaxValue?.Build(),
            MinValue = MinValue?.Build(),
        };
    }
}
