using System.Runtime.CompilerServices;
using FormsApi.Builder.View;
using FormsApi.Form;
using FormsApi.Repository;

[assembly: InternalsVisibleTo("Tests")]

namespace FormsApi.Builder;

public abstract class FormBuilder<TModel>
{
    internal FormModel Build()
    {
        return new FormModel()
        {
            Type = new RepositoryType(typeof(TModel)),
            View = View.Build()
        };
    }

    protected abstract ViewBuilder<TModel> View { get; }
}
