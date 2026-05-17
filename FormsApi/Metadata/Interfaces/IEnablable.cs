using FormsApi.Common;

namespace FormsApi.Metadata.Interfaces;

public interface IEnablable<TModel>
{
    public PropertyOrConstant<TModel, bool>? Enabled { get; set; }
}
public interface IEnablable<TThis, TModel> : IEnablable<TModel>;
