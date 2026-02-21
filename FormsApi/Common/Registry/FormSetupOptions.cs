using System.Collections.Generic;
using FormsApi.Builder;
using FormsApi.Repository;

namespace FormsApi.Common.Registry;

public interface IFormSetupOptions
{
    IFormSetupOptions AddForm(string path, FormBuilder<object> builder);
    IFormSetupOptions AddRepositoryHandler<T>(IRepositoryHandler<T> handler);
}
internal class FormSetupOptions : IFormSetupOptions
{
    private readonly List<KeyValuePair<string, FormBuilder<object>>> _builders = [];
    private readonly List<KeyValuePair<Type, IRepositoryHandler<object>>> _handlers = [];

    public IFormSetupOptions AddForm(string path, FormBuilder<object> builder)
    {
        _builders.Add(new(path, builder));
        return this;
    }

    public IFormSetupOptions AddRepositoryHandler<T>(IRepositoryHandler<T> handler)
    {
        _handlers.Add(new(typeof(T), (IRepositoryHandler<object>)handler));
        return this;
    }

    internal void Configure(FormRegistry forms, RepositoryHandlerRegistry handlers)
    {
        _builders.ForEach(b => forms.Add(b.Key, b.Value));
        _handlers.ForEach(h => handlers.Add(h.Key, h.Value));

    }
}
