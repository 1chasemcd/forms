using System.Linq.Expressions;
using FormsApi.Contract;

namespace FormsApi.Common;

public interface IPropertyOrConstant
{
    PropertyOrConstantDto Build();
}

public sealed class PropertyOrConstant<TModel, TMember> : IPropertyOrConstant
{
    public PropertyOrConstant(TMember? value)
    {
        Value = value;
    }

    public PropertyOrConstant(Expression<Func<TModel, TMember?>> selector)
    {
        Selector = selector;
    }

    public PropertyOrConstantDto Build()
    {
        if (Selector is not null)
            return new PropertyDto(
                new ModelMember<TModel, TMember>(Selector).Build()
            );
        else if (Value is not null)
            return new ConstantDto(Value);

        throw new Exception("PropertyOrConstant has no value");

    }

    public Expression<Func<TModel, TMember?>>? Selector { private get; set; }
    private TMember? Value { get; set; }

    public static implicit operator PropertyOrConstant<TModel, TMember>(TMember value) => new(value);
    public static implicit operator PropertyOrConstant<TModel, TMember>(Expression<Func<TModel, TMember?>> value) => new(value);
}
