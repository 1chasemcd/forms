using System.Linq.Expressions;
using System.Reflection;

internal static class ExpressionExtensions
{
    public static string GetPropertyName<TModel, TProp>(this Expression<Func<TModel, TProp>> selector)
    {
        MemberExpression? memberExpr = selector.Body switch
        {
            MemberExpression m => m,
            UnaryExpression { Operand: MemberExpression m } => m,
            _ => null
        };

        if (memberExpr?.Member is not PropertyInfo prop)
            throw new ArgumentException(
                $"Expression must be a property access", nameof(selector));

        if (memberExpr.Expression is not ParameterExpression)
            throw new ArgumentException(
                $"Expression must access a direct property (no nesting)", nameof(selector));

        return prop.Name;
    }
}
