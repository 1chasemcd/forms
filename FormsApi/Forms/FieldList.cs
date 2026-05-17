using System.Collections;
using System.Linq.Expressions;
using FormsApi.Common;
using FormsApi.Contract;

namespace FormsApi.Forms;

public sealed class FieldList<TModel> : IEnumerable
{
    internal IList<FormControlLayoutDto> Fields { get; } = [];

    public void Add(Expression<Func<TModel, object?>> selector, int? width = null)
    {
        var member = new ModelMember<TModel, object?>(selector);
        Fields.Add(new FormControlLayoutDto
        {
            Property = member.Build(),
            Width = width
        });
    }

    public IEnumerator GetEnumerator() => Fields.GetEnumerator();
}
