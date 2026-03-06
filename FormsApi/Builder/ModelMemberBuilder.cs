using System.Linq.Expressions;
using FormsApi.Builder.Validation;

namespace FormsApi.Builder;

public sealed class ModelMemberBuilder<TModel, TMember>(Expression<Func<TModel, TMember>> selector)
{
    public string Build()
    {
        // Handle boxing to object
        if (selector.Body is UnaryExpression unary &&
            unary.Operand is MemberExpression member1)
            return member1.Member.Name;

        if (selector.Body is MemberExpression member2)
            return member2.Member.Name;

        throw new InvalidOperationException($"Expression '{selector}' must be a member access");
    }

    public static implicit operator ModelMemberBuilder<TModel, TMember>(Expression<Func<TModel, TMember>> selector) => new(selector);

}
