using System.Linq.Expressions;
using FormsApi.Builder.View;
using FormsApi.Definition;

namespace FormsApi.Builder;

public abstract class FormBuilder
{
    internal abstract FormDefinition Build();
}

public abstract class FormBuilder<TModel> : FormBuilder
{
    internal override FormDefinition Build()
    {
        var form = new FormDefinition()
        {
            ModelType = new(typeof(TModel)),
            View = View.Build()
        };

        return form;
    }

    protected abstract ViewBuilder<TModel> View { get; }

    protected static PropertyOrConstantBuilder<TModel, TMember> Property<TMember>(Expression<Func<TModel, TMember?>> selector)
    {
        return new PropertyOrConstantBuilder<TModel, TMember>(selector);
    }

    protected static PropertyOrConstantBuilder<TSubModel, TMember> Property<TSubModel, TMember>(Expression<Func<TSubModel, TMember?>> selector)
    {
        return new PropertyOrConstantBuilder<TSubModel, TMember>(selector);
    }
}
