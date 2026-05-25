using FormsApi.Common;

namespace FormsApi.Metadata.Interfaces;

public interface IServiceMethodCaller<TModel>
{
    IServiceMethodBuilder<TModel>? ServiceMethod { get; set; }
}
public interface IServiceMethodCaller<TThis, TModel> : IServiceMethodCaller<TModel>;
