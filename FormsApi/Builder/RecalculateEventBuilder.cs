using System.Linq.Expressions;
using System.Reflection;
using FormsApi.Form.Primitives;
using FormsApi.Recalculate;

namespace FormsApi.Builder;

public interface IRecalculateEventBuilder<TModel>
{
    RecalculateEvent Build();
}

internal sealed class RecalculateEventBuilder<TModel, TService, TMethod> : IRecalculateEventBuilder<TModel>
{
    private readonly Expression<Func<TService, Func<TMethod, RecalculateEventResult<TMethod>>>>? _methodWithParam;
    private readonly Expression<Func<TService, Func<RecalculateEventResult<TMethod>>>>? _methodNoParam;
    internal RecalculateEventBuilder(Expression<Func<TService, Func<TMethod, RecalculateEventResult<TMethod>>>> method)
    {
        _methodWithParam = method;
    }

    internal RecalculateEventBuilder(Expression<Func<TService, Func<RecalculateEventResult<TMethod>>>> method)
    {
        _methodNoParam = method;
    }

    public RecalculateEvent Build()
    {
        // Wish we could enforce this at compile time
        if (!typeof(TMethod).IsAssignableFrom(typeof(TModel)))
            throw new InvalidOperationException(
                $"Model type '{typeof(TModel).Name}' must be assignable to method parameter/return type '{typeof(TMethod).Name}'");
        return new RecalculateEvent
        {
            Service = new SerializedType(typeof(TService)),
            Method = GetMethodName(),
            PropertiesToSend = GetPropertiesToSend()
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

    private PropertiesToSendCollection GetPropertiesToSend()
    {
        if (_methodWithParam == null && _methodNoParam != null)
            return new SendNone();
        else if (typeof(TModel) == typeof(TMethod))
            return new SendAll();
        else
            return new SendSome()
            {
                Names = GetPublicSettableMemberNames(typeof(TMethod))
            };
    }

    public static IEnumerable<string> GetPublicSettableMemberNames(Type type)
    {
        var properties = type
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.SetMethod != null && p.SetMethod.IsPublic);

        var fields = type
            .GetFields(BindingFlags.Instance | BindingFlags.Public);

        return properties.Cast<MemberInfo>().Concat(fields).Select(x => x.Name);
    }
}
