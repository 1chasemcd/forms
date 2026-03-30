using System;

namespace FormsApi.Builder.Field;

public interface IEnablable<TModel>
{
    public PropertyOrConstantBuilder<TModel, bool>? Enabled { get; set; }

}
