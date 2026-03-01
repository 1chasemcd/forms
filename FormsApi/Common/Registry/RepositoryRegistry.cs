using System.Collections.Concurrent;
using FormsApi.Repository;
using Microsoft.Extensions.Logging;

namespace FormsApi.Common.Registry;

public class RepositoryRegistry(
    ILogger<RepositoryRegistry> logger)
{
    protected readonly ConcurrentDictionary<Type, object> _registry = [];
    internal virtual void Add(Type type, object repository)
    {
        if (!_registry.TryAdd(type, repository))
            logger.LogError("Already had a registration for key '{key}'", type.FullName);
    }

    internal object? TryGet(Type t)
    {
        if (_registry.TryGetValue(t, out object? repository))
            return repository;
        if (GetClosestAncestor(t) is Type ancestor &&
            ancestor != typeof(object) &&
            _registry.TryGetValue(ancestor, out repository))
            return repository;
        return null;
    }

    private Type? GetClosestAncestor(Type type)
    {
        Type closest = typeof(object);
        foreach (Type registeredType in _registry.Keys)
            if (type.IsAssignableTo(registeredType) && registeredType.IsAssignableTo(closest))
                closest = registeredType;

        return closest;
    }
}
