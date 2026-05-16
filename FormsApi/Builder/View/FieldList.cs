using System.Collections;
using System.Linq.Expressions;
using FormsApi.Definition.Field;

namespace FormsApi.Builder.View;

public sealed class FieldList<TModel> : IEnumerable
{
    internal IList<FieldDto> Fields { get; } = [];

    public void Add(Expression<Func<TModel, object?>> selector, int? width = null)
    {
        var member = new ModelMember<TModel, object?>(selector);
        Fields.Add(new FieldDto
        {
            Property = member.Build(),
            Width = width
        });
    }

    public IEnumerator GetEnumerator() => Fields.GetEnumerator();
}
