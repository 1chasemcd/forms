using System;

namespace FormsApi.Repository;

public readonly struct RepositoryType
{
    public string? AssemblyQualifiedName { get; init; }
    internal RepositoryType(Type t)
    {
        AssemblyQualifiedName = t.AssemblyQualifiedName;
    }

    internal Type? GetRepositoryType()
    {
        return Type.GetType(AssemblyQualifiedName ?? string.Empty);
    }
}
