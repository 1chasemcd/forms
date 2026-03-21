using System.Linq.Expressions;
using System.Reflection;
using FormsApi.Form.Primitives;
using FormsApi.Recalculate;

namespace FormsApi.Builder;

public interface IRecalculateEventBuilder<TModel>
{
    RecalculateEvent Build();
}

internal sealed class RecalculateEventBuilder<TModel, TService> : IRecalculateEventBuilder<TModel>
{
    private readonly Expression<Func<TService, Func<TModel, PostRecalculateEvent?>>>? _methodWithParam;
    private readonly Expression<Func<TService, Func<PostRecalculateEvent?>>>? _methodNoParam;
    internal RecalculateEventBuilder(Expression<Func<TService, Func<TModel, PostRecalculateEvent?>>> method)
    {
        _methodWithParam = method;
    }

    internal RecalculateEventBuilder(Expression<Func<TService, Func<PostRecalculateEvent?>>> method)
    {
        _methodNoParam = method;
    }

    public RecalculateEvent Build()
    {
        return new RecalculateEvent
        {
            Service = new SerializedType(typeof(TService)),
            Method = GetMethodName(),
            DontSendModel = _methodWithParam == null && _methodNoParam != null
        };
    }

    private string GetMethodName()
    {
        if (_methodWithParam?.Body is UnaryExpression unary1)
        {
            return GetMethodNameFromUnaryExpression(unary1) ??
                throw new InvalidOperationException($"Expression '{_methodWithParam}' must be a method selector");
        }

        if (_methodNoParam?.Body is UnaryExpression unary2)
        {
            return GetMethodNameFromUnaryExpression(unary2) ??
                throw new InvalidOperationException($"Expression '{_methodNoParam}' must be a method selector");
        }

        throw new Exception("Impossible");
    }

    private static string? GetMethodNameFromUnaryExpression(UnaryExpression unary)
    {
        if (unary.Operand is MethodCallExpression call &&
                call.Object is ConstantExpression constantExpression &&
                constantExpression.Value is MethodInfo m)
            return m.Name;
        return null;
    }
}
