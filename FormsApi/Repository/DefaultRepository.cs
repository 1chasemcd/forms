using System;
using FormsApi.Repository.Query;

namespace FormsApi.Repository;

internal class DefaultRepositoryFactory
{
    public static object? BuildForType(Type type)
    {
        // Must have default constructor
        if (!type.IsValueType && type.GetConstructor(Type.EmptyTypes) == null)
            return null;

        Type closedGeneric = typeof(DefaultRepository<>).MakeGenericType(type);
        return Activator.CreateInstance(closedGeneric);
    }
}

internal class DefaultRepository<T> : IRepository<T>
    where T : new()
{
    public async Task DeleteAsync(T toDelete) => throw new NotImplementedException();
    public async Task<IEnumerable<T>> GetAsync(QueryCriteria criteria) => throw new NotImplementedException();
    public async Task<T> GetNewAsync() => new();
    public Task SaveAsync(T toSave) => throw new NotImplementedException();
}
