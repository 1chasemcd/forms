using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace FormsApi.Common.Registry;

public abstract class BaseRegistry<TKey, TValue>(ILogger<BaseRegistry<TKey, TValue>> logger)
    where TKey : notnull
{
    protected readonly ConcurrentDictionary<TKey, TValue> _registry = [];
    internal void Add(TKey key, TValue value)
    {
        if (!_registry.TryAdd(key, value))
            logger.LogError("Already had a registration for key '{key}'", key);
    }

    internal TValue? TryGet(TKey key)
    {
        _registry.TryGetValue(key, out TValue? value);
        return value;
    }
}
