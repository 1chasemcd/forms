using System.Linq.Expressions;
using System.Reflection;
using FormsApi.Common;
using FormsApi.Form.Field;

namespace FormsApi.Builder.Field;

public sealed class ButtonBuilder<TModel>(
    Expression<Func<TModel, Action>> methodCall)
    : BaseFieldBuilder<TModel, ButtonBuilder<TModel>>
{
    protected override Button BuildField()
    {
        return new Button()
        {
            MethodToRunOnChange = GetMethodName(),
        };
    }

    protected override string GetDefaultLabel() => GetMethodName().CamelCaseToWords();

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
