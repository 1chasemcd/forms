using System;

namespace FormsApi.Builder.Field;

public interface IPrecisionAndScalable<TModel>
{
    PropertyOrConstantBuilder<TModel, int>? Precision { get; set; }
    PropertyOrConstantBuilder<TModel, int>? Scale { get; set; }

}
