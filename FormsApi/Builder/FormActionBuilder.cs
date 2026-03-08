using System.Linq.Expressions;
using System.Reflection;
using FormsApi.Form.Primitives;
using FormsApi.FormAction;

namespace FormsApi.Builder;

public interface IFormActionBuilder<TModel>
{
    Form.Primitives.FormAction Build();
}

internal sealed class FormActionBuilder<TService, TModel> : IFormActionBuilder<TModel>
{
    private readonly Expression<Func<TService, Func<TModel, FormActionResult>>>? _methodWithParam;
    private readonly Expression<Func<TService, Func<FormActionResult>>>? _methodNoParam;
    public FormActionBuilder(Expression<Func<TService, Func<TModel, FormActionResult>>> method)
    {
        _methodWithParam = method;
    }

    public FormActionBuilder(Expression<Func<TService, Func<FormActionResult>>> method)
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
