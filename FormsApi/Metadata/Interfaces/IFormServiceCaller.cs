using FormsApi.Common;

namespace FormsApi.Metadata.Interfaces;

public interface IFormServiceCaller<TModel>
{
    IFormServiceMethod<TModel>? FormServiceMethod { get; set; }
}
public interface IRecalculatable<TThis, TModel> : IFormServiceCaller<TModel>;
