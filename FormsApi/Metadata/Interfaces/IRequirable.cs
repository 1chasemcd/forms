using FormsApi.Common;

namespace FormsApi.Metadata.Interfaces;

public interface IRequirable<TModel>
{
    PropertyOrConstant<TModel, bool>? Required { get; set; }
}
public interface IRequirable<TThis, TModel> : IRequirable<TModel>;
