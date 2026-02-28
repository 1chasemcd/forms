using FormsApi.Builder.View;
using FormsApi.Common.Registry;
using FormsApi.Form;
using FormsApi.Form.Primitives;

namespace FormsApi.Builder;

public abstract class FormBuilder
{
    internal abstract FormModel Build();
}

public abstract class FormBuilder<TModel> : FormBuilder
{
    internal override FormModel Build()
    {
        return new FormModel()
        {
            Type = new(typeof(TModel)),
            View = View.Build()
        };
    }

    protected abstract ViewBuilder<TModel> View { get; }
}
