using System.Collections.Generic;
using FormsApi.Builder;
using FormsApi.Repository;

namespace FormsApi.Common.Registry;

public interface IFormSetupOptions
{
    public IFormSetupOptions AddForm<TModel>(string path, FormBuilder<TModel> builder);
    IFormSetupOptions AddRepositoryHandler<T>(IRepositoryHandler<T> handler);
}
internal class FormSetupOptions : IFormSetupOptions
{
    private readonly List<KeyValuePair<string, FormBuilder>> _builders = [];
    private readonly List<KeyValuePair<Type, IRepositoryHandler<object>>> _handlers = [];

    public IFormSetupOptions AddForm<TModel>(string path, FormBuilder<TModel> builder)
    {
        _builders.Add(new(path, builder));
        return this;
    }

    public IFormSetupOptions AddRepositoryHandler<T>(IRepositoryHandler<T> handler)
    {
        _handlers.Add(new(typeof(T), (IRepositoryHandler<object>)handler));
        return this;
    }

    internal void Configure(FormRegistry forms, RepositoryHandlerRegistry handlers, RepositoryTypeRegistry repoTypes)
    {
        _builders.ForEach(b => forms.Add(b.Key, b.Value.Build(repoTypes)));
        _handlers.ForEach(h => handlers.Add(h.Key, h.Value));

    }
}
