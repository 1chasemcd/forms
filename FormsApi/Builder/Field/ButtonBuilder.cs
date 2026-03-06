using System.Linq.Expressions;
using System.Reflection;
using FormsApi.Builder.Validation;
using FormsApi.Form.Field;

namespace FormsApi.Builder.Field;

public sealed class ButtonBuilder<TModel>(
    Expression<Func<TModel, Action>> methodCall)
    : BaseFieldBuilder<TModel, ButtonBuilder<TModel>>
{
    protected override Button BuildImpl()
    {
        return new Button()
        {
            MethodToRunOnChange = GetMethodName(),
        };
    }

    private string GetMethodName()
    {
        if (methodCall.Body is UnaryExpression unary &&
            unary.Operand is MethodCallExpression call &&
            call.Object is ConstantExpression constantExpression &&
            constantExpression.Value is MethodInfo method)
            return method.Name;

        throw new InvalidOperationException($"Expression '{methodCall}' must be a method call.");
    }
}
