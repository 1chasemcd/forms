using System.Security.Cryptography;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using FormsApi.Form.Primitives;


namespace FormsApi.Common.Registry;

public class RepositoryTypeRegistry
{
    private readonly ConcurrentDictionary<Type, string> _typeToHash = [];
    private readonly ConcurrentDictionary<string, Type> _hashToType = [];

    internal RepositoryType Add<T>()
    {
        Type type = typeof(T);
        // Do not register twice
        if (_typeToHash.TryGetValue(type, out string? repoType))
            return new RepositoryType(repoType);

        string name = type.AssemblyQualifiedName ?? throw new Exception("No assembly name");

        string hash = ComputeHash(name);

        // Try to avoid collision
        int attempt = 0;
        while (_hashToType.ContainsKey(hash))
        {
            attempt++;
            name += "$";
            hash = ComputeHash(name);
            if (attempt > 10)
                throw new Exception($"Could not hash type '{type.Name}' without collision");
        }
        _hashToType[hash] = type;
        _typeToHash[type] = hash;

        return new RepositoryType(hash);
    }

    internal RepositoryType? TryGetRepositoryType<T>()
    {
        if (_typeToHash.TryGetValue(typeof(T), out string? value))
            return new RepositoryType(value);

        return null;
    }

    internal Type? TryGetRuntimeType(RepositoryType repositoryType)
    {
        _hashToType.TryGetValue(repositoryType.TypeId, out Type? type);
        return type;
    }

    private static string ComputeHash(string text)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(text));

        // shorten to first 8 bytes (out of 32 total)
        return WebEncoders.Base64UrlEncode(bytes[..8]);
    }
}
