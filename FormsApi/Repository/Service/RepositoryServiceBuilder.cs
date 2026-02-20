using System.Reflection;

namespace FormsApi.Repository.Service;

public class RepositoryServiceBuilder
(IRepositoryHandlerRegistry registry)
{
    internal IReadableRepositoryService BuildWithType(RepositoryType type)
    {
        return (IReadableRepositoryService)BuildService(type, typeof(ReadableRepositoryService<>));
    }

    internal IWriteableRepositoryService BuildWithTypeAndObject(RepositoryType type, object obj)
    {
        return (IWriteableRepositoryService)BuildService(type, typeof(WriteableRepositoryService<>), obj);
    }

    private object BuildService(RepositoryType type, Type serviceType, object? obj = null)
    {
        Type repositoryType = type.GetRepositoryType() ?? throw new Exception("Invalid type");
        object handler = GetHandler(repositoryType) ?? throw new Exception("No repository handler for type");
        Type closedGeneric = serviceType.MakeGenericType(repositoryType);
        object? service = obj is null ?
            Activator.CreateInstance(closedGeneric, handler) :
            Activator.CreateInstance(closedGeneric, handler, obj);
        return service ?? throw new Exception("Could not construct service");
    }

    private object? GetHandler(Type t)
    {
        MethodInfo genericMethod = typeof(IRepositoryHandlerRegistry).GetMethod(nameof(IRepositoryHandlerRegistry.Get)) ?? throw new Exception();
        MethodInfo getMethod = genericMethod.MakeGenericMethod(t);
        return getMethod.Invoke(registry, null);
    }
}
