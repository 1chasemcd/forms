using System.Linq.Expressions;
using System.Reflection;
using FormsApi.Form.Primitives;
using FormsApi.Recalculate;

namespace FormsApi.Builder;

public interface IFormActionBuilder<TModel>
{
    Form.Primitives.FormAction Build();
}

internal sealed class FormActionBuilder<TService, TModel> : IFormActionBuilder<TModel>
{
    private readonly Expression<Func<TService, Func<TModel, RecalculateEventResult>>>? _methodWithParam;
    private readonly Expression<Func<TService, Func<RecalculateEventResult>>>? _methodNoParam;
    public FormActionBuilder(Expression<Func<TService, Func<TModel, RecalculateEventResult>>> method)
    {
        _methodWithParam = method;
    }

    public FormActionBuilder(Expression<Func<TService, Func<RecalculateEventResult>>> method)
    {
        _methodNoParam = method;
    }
    public Form.Primitives.FormAction Build()
    {
        return new Form.Primitives.FormAction
        {
            Service = new SerializedType(typeof(TService)),
            Method = GetMethodName()
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
