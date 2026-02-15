using System.Collections.Concurrent;

namespace FormsApi.Repository;

public interface IRepositoryHandlerRegistry
{
    public void Register<T>(IRepositoryHandler<T> handler);
    internal IRepositoryHandler<T>? Get<T>();
}

public class RepositoryHandlerRegistry : IRepositoryHandlerRegistry
{
    private readonly ConcurrentDictionary<Type, IRepositoryHandler<object>> _registry = new();
    public void Register<T>(IRepositoryHandler<T> handler)
    {
        _registry[typeof(T)] = (IRepositoryHandler<object>)handler;
    }

    public IRepositoryHandler<T>? Get<T>()
    {
        return _registry.TryGetValue(typeof(T), out var result) ? (IRepositoryHandler<T>)result : null;
    }
}
