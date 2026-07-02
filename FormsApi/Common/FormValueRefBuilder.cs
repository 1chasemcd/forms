using System.Diagnostics;
using System.Linq.Expressions;
using FormsApi.Contract;

namespace FormsApi.Common;

public interface IFormValueRefBuilder
{
    FormValueRef Build();
}

public sealed class FormValueRefBuilder<TModel, TMember> : IFormValueRefBuilder
{
    public FormValueRefBuilder(TMember value)
    {
        ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    public FormValueRefBuilder(Expression<Func<TModel, TMember?>> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);
        _selector = selector;
    }

    public FormValueRef Build()
    {
        if (_selector is not null)
            return new ModelValue(_selector.GetPropertyName());
        else if (_value is not null)
            return new ConstantValue(_value);

        throw new UnreachableException("FormValueRef has no value");

    }

    private readonly Expression<Func<TModel, TMember?>>? _selector;
    private readonly TMember? _value;

    public static implicit operator FormValueRefBuilder<TModel, TMember>(TMember value) => new(value);
    public static implicit operator FormValueRefBuilder<TModel, TMember>(Expression<Func<TModel, TMember?>> value) => new(value);
}
