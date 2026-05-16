using System;

namespace FormsApi.Builder.Metadata;

public interface IVisible<TModel>
{
    public PropertyOrConstant<TModel, bool>? Visible { get; set; }

}
public interface IVisible<TThis, TModel> : IVisible<TModel>;
