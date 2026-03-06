using FormsApi.Builder.Validation;
using FormsApi.Builder.View;
using FormsApi.Common.Registry;
using FormsApi.Form;
using FormsApi.Form.Primitives;

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
            Type = new(typeof(TModel)),
            View = View.Build()
        };

        return form;
    }

    protected abstract ViewBuilder<TModel> View { get; }
}
