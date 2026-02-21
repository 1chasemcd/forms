using FormsApi.Builder.View;
using FormsApi.Common.Registry;
using FormsApi.Form;
using FormsApi.Repository;

namespace FormsApi.Builder;

public abstract class FormBuilder
{
    internal abstract FormModel Build(RepositoryTypeRegistry typeRegistr);
}

public abstract class FormBuilder<TModel> : FormBuilder
{
    internal override FormModel Build(RepositoryTypeRegistry typeRegistry)
    {
        RepositoryType repositoryType = typeRegistry.Add<TModel>();

        return new FormModel()
        {
            Type = repositoryType,
            View = View.Build()
        };
    }

    protected abstract ViewBuilder<TModel> View { get; }
}
