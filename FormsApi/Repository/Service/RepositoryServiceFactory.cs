using System.Text.Json;
using FormsApi.Form.Primitives;

namespace FormsApi.Repository.Service;

public interface IRepositoryServiceFactory
{
    IReadableRepositoryService BuildWithType(RepositoryType type);
    IWriteableRepositoryService BuildWithTypeAndObject(RepositoryType type, JsonElement body);
}

internal sealed class RepositoryServiceFactory(IRepositoryResolver resolver) : IRepositoryServiceFactory
{
    public IReadableRepositoryService BuildWithType(RepositoryType type)
    {
        Type objectType = type.GetRuntimeType();
        object repository = resolver.Resolve(objectType);
        Type repositoryType = GetRepositoryTypeArgument(repository);
        Type closedGeneric = typeof(ReadableRepositoryService<>).MakeGenericType(repositoryType);
        object? service = Activator.CreateInstance(closedGeneric, repository);
        return service as IReadableRepositoryService ?? throw new InvalidOperationException("Could not construct service");
    }

    public IWriteableRepositoryService BuildWithTypeAndObject(RepositoryType type, JsonElement body)
    {
        Type objectType = type.GetRuntimeType();
        object repository = resolver.Resolve(objectType);
        Type repositoryType = GetRepositoryTypeArgument(repository);
        Type closedGeneric = typeof(WriteableRepositoryService<,>).MakeGenericType(repositoryType, objectType);
        object? obj = body.Deserialize(objectType);
        object? service = Activator.CreateInstance(closedGeneric, repository, obj);
        return service as IWriteableRepositoryService ?? throw new InvalidOperationException("Could not construct service");
    }

    private static Type GetRepositoryTypeArgument(object repository)
    {
        Type type = repository.GetType();

        Type? repoInterface = type
            .GetInterfaces()
            .SingleOrDefault(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IRepository<>))
                ?? throw new InvalidOperationException(
                    $"Object of type {repository.GetType().Name} does not implement IRepository<T>");

        return repoInterface.GetGenericArguments()[0];
    }
}
