using System;

namespace FormsApi.Builder.Field;

public interface IValueRangable<TModel, TField>
{
    PropertyOrConstantBuilder<TModel, TField>? MaxValue { get; set; }
    PropertyOrConstantBuilder<TModel, TField>? MinValue { get; set; }
}
