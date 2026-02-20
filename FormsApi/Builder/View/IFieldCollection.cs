using System.Collections;
using FormsApi.Builder.Field;

namespace FormsApi.Builder.View;

public interface IFieldCollection<TModel> : IEnumerable
{
    IList<BaseFieldBuilder<TModel>> Fields { get; }
}
