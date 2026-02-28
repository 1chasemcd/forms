using FormsApi.Builder;
using FormsApi.Repository;

namespace FormsApi.Common.Registry;

public interface IFormSetupOptions
{
    public IFormSetupOptions AddForm<TModel>(string path, FormBuilder<TModel> builder);
    IFormSetupOptions AddRepository<T>(IRepository<T> repository);
}
internal class FormSetupOptions : IFormSetupOptions
{
    private readonly List<KeyValuePair<string, FormBuilder>> _builders = [];
    private readonly List<KeyValuePair<Type, IRepository<object>>> _repositories = [];

    public IFormSetupOptions AddForm<TModel>(string path, FormBuilder<TModel> builder)
    {
        _builders.Add(new(path, builder));
        return this;
    }

    public IFormSetupOptions AddRepository<T>(IRepository<T> repository)
    {
        _repositories.Add(new(typeof(T), (IRepository<object>)repository));
        return this;
    }

    internal void Configure(FormRegistry forms, RepositoryRegistry repositories)
    {
        _builders.ForEach(b => forms.Add(b.Key, b.Value.Build()));
        _repositories.ForEach(h => repositories.Add(h.Key, h.Value));

    }
}
