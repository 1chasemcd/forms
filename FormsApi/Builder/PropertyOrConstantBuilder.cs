using System.Linq.Expressions;
using FormsApi.Form.Primitives;

namespace FormsApi.Builder;

public sealed class PropertyOrConstantBuilder<TModel, TMember>
    where TMember : notnull
{
    public PropertyOrConstantBuilder(TMember value)
    {
        Value = value;
    }

    public PropertyOrConstantBuilder(Expression<Func<TModel, TMember>> selector)
    {
        Selector = selector;
    }

    internal PropertyOrConstant Build()
    {
        if (Selector is not null)
            return new Property(
                new ModelMemberBuilder<TModel, TMember>(Selector).Build()
            );
        else if (Value is not null)
            return new Constant(Value);

        throw new Exception("PropertyOrConstant has no value");

    }

    public Expression<Func<TModel, TMember>>? Selector { private get; set; }
    private TMember? Value { get; set; }

    public static implicit operator PropertyOrConstantBuilder<TModel, TMember>(TMember value) => new(value);
    public static implicit operator PropertyOrConstantBuilder<TModel, TMember>(Expression<Func<TModel, TMember>> value) => new(value);
}
