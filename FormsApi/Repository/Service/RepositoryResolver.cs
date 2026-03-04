using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace FormsApi.Repository.Service;

public interface IRepositoryResolver
{
    object Resolve(Type modelType);
}

internal sealed class RepositoryResolver(IServiceProvider provider) : IRepositoryResolver
{
    private readonly ConcurrentDictionary<Type, object> _cache = new();

    public object Resolve(Type modelType)
    {
        return _cache.GetOrAdd(modelType, ResolveInternal);
    }

    private object ResolveInternal(Type modelType)
    {
        Type? current = modelType;

        while (current != null)
        {
            object? repo = TryResolve(current);
            if (repo != null) return repo;
            current = current.BaseType;
        }

        foreach (Type iface in modelType.GetInterfaces())
        {
            object? repo = TryResolve(iface);
            if (repo != null) return repo;
        }

        if (CreateDefaultRepository(modelType) is { } defaultRepository)
            return defaultRepository;

        throw new InvalidOperationException(
            $"No IRepository<T> found for {modelType}");
    }

    private object? TryResolve(Type type)
    {
        var repoType = typeof(IRepository<>).MakeGenericType(type);
        return provider.GetService(repoType);
    }

    private object? CreateDefaultRepository(Type modelType)
    {
        // Must have default constructor
        if (!modelType.IsValueType && modelType.GetConstructor(Type.EmptyTypes) == null)
            return null;
        Type repoType = typeof(DefaultRepository<>).MakeGenericType(modelType);
        return ActivatorUtilities.CreateInstance(provider, repoType);
    }
}
