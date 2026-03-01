using System.Reflection;
using FormsApi.Common.Registry;
using FormsApi.Form.Primitives;

namespace FormsApi.Repository.Service;

public sealed class RepositoryServiceBuilder(RepositoryRegistry repositoryRegistry)
{
    internal IReadableRepositoryService BuildWithType(RepositoryType type)
    {
        Type objectType = type.GetRuntimeType() ?? throw new Exception("Invalid type");
        object repository = GetRepository(objectType) ?? throw new Exception("No repository for type");
        Type repositoryType = GetRepositoryTypeArgument(repository) ?? throw new Exception("Could not get type argument for repository");
        Type closedGeneric = typeof(ReadableRepositoryService<>).MakeGenericType(repositoryType);
        object? service = Activator.CreateInstance(closedGeneric, repository);
        return service as IReadableRepositoryService ?? throw new Exception("Could not construct service");
    }

    internal IWriteableRepositoryService BuildWithTypeAndObject(RepositoryType type, object obj)
    {
        Type objectType = type.GetRuntimeType() ?? throw new Exception("Invalid type");
        object repository = GetRepository(objectType) ?? throw new Exception("No repository for type");
        Type repositoryType = GetRepositoryTypeArgument(repository) ?? throw new Exception("Could not get type argument for repository");
        Type closedGeneric = typeof(WriteableRepositoryService<,>).MakeGenericType(repositoryType, objectType);
        object? service = Activator.CreateInstance(closedGeneric, repository, obj);
        return service as IWriteableRepositoryService ?? throw new Exception("Could not construct service");
    }

    private object? GetRepository(Type t)
    {
        return repositoryRegistry.TryGet(t) ?? GetDefaultRepository(t);
    }

    private static object? GetDefaultRepository(Type t)
    {
        // Must have default constructor
        if (!t.IsValueType && t.GetConstructor(Type.EmptyTypes) == null)
            return null;

        Type closedGeneric = typeof(DefaultRepository<>).MakeGenericType(t);
        return Activator.CreateInstance(closedGeneric);
    }

    private static Type? GetRepositoryTypeArgument(object repository)
    {
        Type type = repository.GetType();

        Type? repoInterface = type
            .GetInterfaces()
            .FirstOrDefault(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IRepository<>));

        return repoInterface?.GetGenericArguments()[0];
    }
}
