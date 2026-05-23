using System.Linq.Expressions;
using System.Reflection;
using FormsApi.Contract;
using FormsApi.FormService;

namespace FormsApi.Common;

public interface IFormServiceMethodBuilder<TModel>
{
    FormServiceMethod Build();
}
public sealed class FormServiceMethodBuilder<TModel, TService>(Expression<Func<TService, Func<TModel, FormServicePostAction?>>> method) : IFormServiceMethodBuilder<TModel>
{

    public FormServiceMethod Build()
    {
        return new FormServiceMethod
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
