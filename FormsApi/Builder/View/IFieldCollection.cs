using System.Collections;
using FormsApi.Builder.Field;

namespace FormsApi.Builder.View;

public interface IFieldCollection<TModel> : IEnumerable
{
    IList<IFieldBuilder<TModel>> Fields { get; }
}
