using System.Collections;
using System.Linq.Expressions;
using FormsApi.Common;
using FormsApi.Contract;

namespace FormsApi.Forms;

public sealed class ControlList<TModel> : IEnumerable<FormControlLayoutDto>
{
    internal IList<FormControlLayoutDto> Controls { get; } = [];

    public void Add(Expression<Func<TModel, object?>> selector, int? width = null)
    {
        var member = new ModelMember<TModel, object?>(selector);
        Controls.Add(new FormControlLayoutDto
        {
            PropertyName = member.Build(),
            Width = width
        });
    }

    IEnumerator<FormControlLayoutDto> IEnumerable<FormControlLayoutDto>.GetEnumerator() => Controls.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Controls.GetEnumerator();
}
