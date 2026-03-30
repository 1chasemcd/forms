using System;

namespace FormsApi.Builder.Field;

public interface IRequirable<TModel>
{
    PropertyOrConstantBuilder<TModel, bool>? Required { get; set; }
}
