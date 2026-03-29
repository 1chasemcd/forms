using System.Text.Json;
using FormsApi.Definition.Primitives;
using FormsApi.Repository.Handler;
using FormsApi.Repository.Query;

namespace FormsApi.Repository.Service;

public interface IRepositoryServiceFactory
{
    IRepositoryCallable BuildCreateService(SerializedType type);
    IRepositoryCallable BuildDeleteService(SerializedType type, object model);
    IRepositoryCallable BuildQueryService(SerializedType type, QueryCriteria criteria);
    IRepositoryCallable BuildSaveService(SerializedType type, object model);
}

internal sealed class RepositoryServiceFactory(IRepositoryResolver resolver) : IRepositoryServiceFactory
{
    public IRepositoryCallable BuildCreateService(SerializedType type) =>
        Build(typeof(RepositoryCreateService<>), typeof(IRepositoryCreateHandler<>), type);
    public IRepositoryCallable BuildDeleteService(SerializedType type, object model) =>
        Build(typeof(RepositoryDeleteService<>), typeof(IRepositoryDeleteHandler<>), type, model);
    public IRepositoryCallable BuildQueryService(SerializedType type, QueryCriteria criteria) =>
        Build(typeof(RepositoryQueryService<>), typeof(IRepositoryQueryHandler<>), type, criteria);
    public IRepositoryCallable BuildSaveService(SerializedType type, object model) =>
        Build(typeof(RepositorySaveService<>), typeof(IRepositorySaveHandler<>), type, model);

    private IRepositoryCallable Build(Type serviceType, Type handlerType, SerializedType modelType, params object[] additionalArgs)
    {
        object repository = resolver.Resolve(handlerType, modelType.GetRuntimeType());
        Type repositoryType = GetHandlerTypeArgument(handlerType, repository);
        Type closedGeneric = serviceType.MakeGenericType(repositoryType);
        object[] args = [.. additionalArgs.AsEnumerable().Prepend(repository)];
        object? service = Activator.CreateInstance(closedGeneric, args);
        return service as IRepositoryCallable ?? throw new InvalidOperationException("Could not construct service");
    }

    private static Type GetHandlerTypeArgument(Type interfaceType, object handler)
    {
        Type type = handler.GetType();

        Type? repoInterface = type
            .GetInterfaces()
            .SingleOrDefault(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == interfaceType)
                ?? throw new InvalidOperationException(
                    $"Object of type {handler.GetType().Name} does not implement IRepository<T>");

        return repoInterface.GetGenericArguments()[0];
    }
}
