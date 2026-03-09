using System.Collections.Concurrent;
using System.Runtime;
using FormsApi.Repository.Handler;
using Microsoft.Extensions.DependencyInjection;

namespace FormsApi.Repository.Service;

public interface IRepositoryResolver
{
    object Resolve(Type handlerType, Type modelType);
}

internal sealed class RepositoryResolver(IServiceProvider provider) : IRepositoryResolver
{
    private readonly ConcurrentDictionary<Type, IEnumerable<object>> _cache = new();

    public object Resolve(Type handlerType, Type modelType)
    {
        var types = _cache.GetOrAdd(modelType, ResolveInternal)
        .Where(r => r.GetType().GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == handlerType
            )).ToList();

        if (types.Count > 1)
            throw new InvalidOperationException($"Ambiguous repository handler implementations found for type {modelType}: {string.Join(", ", types)}");
        if (types.Count == 0)
            throw new InvalidOperationException($"No {handlerType} implementation found for type {modelType}");
        return types.Single();
    }

    private IEnumerable<object> ResolveInternal(Type modelType)
    {
        IEnumerable<object> matches = [];

        Type? current = modelType;

        while (current != null)
        {
            matches = matches.Concat(TryResolve(current));
            current = current.BaseType;
        }

        foreach (Type iface in modelType.GetInterfaces())
            matches = matches.Concat(TryResolve(iface));

        if (!AlreadyHasCreateHandler(matches) && CreateDefaultRepository(modelType) is { } dr)
            matches = matches.Append(dr);

        return matches;
    }

    private IEnumerable<object> TryResolve(Type type)
    {
        Type[] repoTypes = [
            typeof(IRepositoryCreateHandler<>),
            typeof(IRepositorySaveHandler<>),
            typeof(IRepositoryQueryHandler<>),
            typeof(IRepositoryDeleteHandler<>)];

        foreach (Type option in repoTypes)
            if (provider.GetService(option.MakeGenericType(type)) is { } service)
                yield return service;
    }

    private static bool AlreadyHasCreateHandler(IEnumerable<object> matches)
    {
        return matches.Any(o => o.GetType().GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IRepositoryCreateHandler<>)));
    }

    private object? CreateDefaultRepository(Type modelType)
    {
        // Must have default constructor
        if (!modelType.IsValueType && modelType.GetConstructor(Type.EmptyTypes) == null)
            return null;
        Type repoType = typeof(DefaultRepositoryCreateHandler<>).MakeGenericType(modelType);
        return ActivatorUtilities.CreateInstance(provider, repoType);
    }
}
