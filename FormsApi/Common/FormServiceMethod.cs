using System.Linq.Expressions;
using System.Reflection;
using FormsApi.Contract;
using FormsApi.FormService;

namespace FormsApi.Common;

public interface IFormServiceMethod<TModel>
{
    FormServiceMethodDto Build();
}
public sealed class FormServiceMethod<TModel, TService>(Expression<Func<TService, Func<TModel, FormServicePostAction?>>> method) : IFormServiceMethod<TModel>
{

    public FormServiceMethodDto Build()
    {
        return new FormServiceMethodDto
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
