using System.Diagnostics;
using System.Linq.Expressions;
using FormsApi.Contract;

namespace FormsApi.Common;

public interface IPropertyOrConstantBuilder
{
    PropertyOrConstant Build();
}

public sealed class PropertyOrConstantBuilder<TModel, TMember> : IPropertyOrConstantBuilder
{
    public PropertyOrConstantBuilder(TMember value)
    {
        ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    public PropertyOrConstantBuilder(Expression<Func<TModel, TMember?>> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);
        _selector = selector;
    }

    public PropertyOrConstant Build()
    {
        if (_selector is not null)
            return new Property(_selector.GetPropertyName());
        else if (_value is not null)
            return new Constant(_value);

        throw new UnreachableException("PropertyOrConstant has no value");

    }

    private readonly Expression<Func<TModel, TMember?>>? _selector;
    private readonly TMember? _value;

    public static implicit operator PropertyOrConstantBuilder<TModel, TMember>(TMember value) => new(value);
    public static implicit operator PropertyOrConstantBuilder<TModel, TMember>(Expression<Func<TModel, TMember?>> value) => new(value);
}
