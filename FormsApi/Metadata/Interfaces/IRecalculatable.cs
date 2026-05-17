using FormsApi.Common;

namespace FormsApi.Metadata.Interfaces;

public interface IRecalculatable<TModel>
{
    IRecalculateEvent<TModel>? RecalculateEvent { get; set; }
}
public interface IRecalculatable<TThis, TModel> : IRecalculatable<TModel>;
