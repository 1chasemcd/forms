using System.Diagnostics;
using System.Linq.Expressions;
using FormsApi.Contract;

namespace FormsApi.Common;

public interface IPropertyOrConstant
{
    PropertyOrConstantDto Build();
}

public sealed class PropertyOrConstant<TModel, TMember> : IPropertyOrConstant
{
    public PropertyOrConstant(TMember value)
    {
        ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    public PropertyOrConstant(Expression<Func<TModel, TMember?>> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);
        _selector = selector;
    }

    public PropertyOrConstantDto Build()
    {
        if (_selector is not null)
            return new PropertyDto(_selector.GetPropertyName());
        else if (_value is not null)
            return new ConstantDto(_value);

        throw new UnreachableException("PropertyOrConstant has no value");

    }

    private readonly Expression<Func<TModel, TMember?>>? _selector;
    private readonly TMember? _value;

    public static implicit operator PropertyOrConstant<TModel, TMember>(TMember value) => new(value);
    public static implicit operator PropertyOrConstant<TModel, TMember>(Expression<Func<TModel, TMember?>> value) => new(value);
}
