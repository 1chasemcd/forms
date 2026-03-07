using FormsApi.Builder.View;
using FormsApi.Form;

namespace FormsApi.Builder;

public abstract class FormBuilder
{
    internal abstract BaseForm Build();
}

public abstract class FormBuilder<TModel> : FormBuilder
{
    internal override BaseForm Build()
    {
        var form = new BaseForm()
        {
            Type = new(typeof(TModel)),
            View = View.Build()
        };

        return form;
    }

    protected abstract ViewBuilder<TModel> View { get; }
}
