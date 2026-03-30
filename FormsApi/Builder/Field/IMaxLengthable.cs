using System;

namespace FormsApi.Builder.Field;

public interface IMaxLengthable<TModel>
{
    PropertyOrConstantBuilder<TModel, int>? MaxLength { get; set; }

}
