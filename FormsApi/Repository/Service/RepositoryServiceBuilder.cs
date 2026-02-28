using System.Reflection;
using FormsApi.Common.Registry;
using FormsApi.Form.Primitives;

namespace FormsApi.Repository.Service;

public class RepositoryServiceBuilder
    (RepositoryRegistry repositoryRegistry)
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
        Type repositoryType = type.GetRuntimeType() ?? throw new Exception("Invalid type");
        object repository = GetRepository(repositoryType) ?? throw new Exception("No repository for type");
        Type closedGeneric = serviceType.MakeGenericType(repositoryType);
        object? service = obj is null ?
            Activator.CreateInstance(closedGeneric, repository) :
            Activator.CreateInstance(closedGeneric, repository, obj);
        return service ?? throw new Exception("Could not construct service");
    }

    private object? GetRepository(Type t)
    {
        MethodInfo genericMethod = typeof(RepositoryRegistry).GetMethod(nameof(RepositoryRegistry.TryGet)) ?? throw new Exception();
        MethodInfo getMethod = genericMethod.MakeGenericMethod(t);
        return getMethod.Invoke(repositoryRegistry, null);
    }
}
