using System.Collections.Concurrent;

namespace FormsApi.Repository;

public class RepositoryHandlerRegistry
{
    private readonly ConcurrentDictionary<Type, IRepositoryHandler<object>> _registry = new();
    internal void Add(Type type, IRepositoryHandler<object> handler)
    {
        _registry[type] = handler;
    }

    internal IRepositoryHandler<T>? Get<T>()
    {
        return _registry.TryGetValue(typeof(T), out IRepositoryHandler<object>? result) ?
            (IRepositoryHandler<T>)result :
            null;
    }
}
