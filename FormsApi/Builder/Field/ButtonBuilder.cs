using System.Linq.Expressions;
using System.Reflection;
using FormsApi.Form.Field;

namespace FormsApi.Builder.Field;

public class ButtonBuilder<TModel>(
    Expression<Func<TModel, Action>> methodCall)
    : BaseFieldBuilder<TModel>
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

        throw new FormBuilderValidationException<TModel>(methodCall.ToString(), "Expression must be a method call.");
    }
}
