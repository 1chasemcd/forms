using FormsApi.Common;

namespace FormsApi.Metadata.Interfaces;

public interface IFormServiceCaller<TModel>
{
    IFormServiceMethodBuilder<TModel>? FormServiceMethod { get; set; }
}
public interface IRecalculatable<TThis, TModel> : IFormServiceCaller<TModel>;
