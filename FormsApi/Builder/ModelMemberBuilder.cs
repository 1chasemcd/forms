using System.Linq.Expressions;

namespace FormsApi.Builder;

public class ModelMemberBuilder<TModel, TMember>(Expression<Func<TModel, TMember>> selector)
{
    public enum MemberAccessType
    {
        PropertyOrField,
        Method
    }
    public string Build(MemberAccessType accessType = MemberAccessType.PropertyOrField)
    {
        // Handle boxing to object
        if (selector.Body is UnaryExpression unary &&
            unary.Operand is MemberExpression member1)
            return member1.Member.Name;

        if (selector.Body is MemberExpression member2)
            return member2.Member.Name;

        throw new FormBuilderValidationException<TModel>(selector.ToString(), "Expression must be a member access");
    }

    internal MemberExpression? GetMember()
    {
        if (selector.Body is UnaryExpression unary &&
            unary.Operand is MemberExpression member1)
            return member1;

        if (selector.Body is MemberExpression member2)
            return member2;

        return null;
    }

    public static implicit operator ModelMemberBuilder<TModel, TMember>(Expression<Func<TModel, TMember>> selector) => new(selector);

}
