using System.Linq.Expressions;
using System.Reflection;
using FormsApi.Contract;
using FormsApi.Contract.PostRequest;

namespace FormsApi.Common;

public interface IServiceMethodBuilder<TModel>
{
    Contract.ServiceMethod Build();
}
public sealed class ServiceMethodBuilder<TModel, TService>(Expression<Func<TService, Func<TModel, PostRequestAction?>>> method) : IServiceMethodBuilder<TModel>
{

    public Contract.ServiceMethod Build()
    {
        return new Contract.ServiceMethod
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
