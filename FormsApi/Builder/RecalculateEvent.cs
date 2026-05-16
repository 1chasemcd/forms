using System.Linq.Expressions;
using System.Reflection;
using FormsApi.Definition.Primitives;
using FormsApi.Recalculate;

namespace FormsApi.Builder;

public interface IRecalculateEvent<TModel>
{
    RecalculateEventDto Build();
}
public sealed class RecalculateEvent<TModel, TService>(Expression<Func<TService, Func<TModel, PostRecalculateEvent?>>> method) : IRecalculateEvent<TModel>
{

    public RecalculateEventDto Build()
    {
        return new RecalculateEventDto
        {
            Service = new TypeDto(typeof(TService)),
            Method = GetMethodName(),
        };
    }

    private string GetMethodName()
    {
        if (method.Body is UnaryExpression unary &&
            unary.Operand is MethodCallExpression call &&
            call.Object is ConstantExpression constantExpression &&
            constantExpression.Value is MethodInfo m)
            return m.Name;
        throw new InvalidOperationException($"Expression '{method}' must be a method selector");
    }
}
