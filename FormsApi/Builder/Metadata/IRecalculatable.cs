using System.Linq.Expressions;
using FormsApi.Recalculate;

namespace FormsApi.Builder.Metadata;

public interface IRecalculatable<TModel>
{
    IRecalculateEvent<TModel>? RecalculateEvent { get; set; }
}
public interface IRecalculatable<TThis, TModel> : IRecalculatable<TModel>;
