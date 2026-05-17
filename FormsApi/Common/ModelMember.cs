using System.Linq.Expressions;
using System.Reflection;

namespace FormsApi.Common;

public sealed class ModelMember<TModel, TMember>(Expression<Func<TModel, TMember?>> selector)
{
    public string Build()
    {

        var memberExpr = selector.Body switch
        {
            MemberExpression m => m,
            UnaryExpression { Operand: MemberExpression m } => m,
            _ => null
        };

        if (memberExpr?.Member is not PropertyInfo prop)
            throw new InvalidOperationException(
                $"Expression '{selector}' must be a property access");

        if (memberExpr.Expression is not ParameterExpression)
            throw new InvalidOperationException(
                $"Expression '{selector}' must access a direct property (no nesting)");

        return prop.Name;
    }

    public static implicit operator ModelMember<TModel, TMember>(Expression<Func<TModel, TMember?>> selector1) => new(selector1);


}
