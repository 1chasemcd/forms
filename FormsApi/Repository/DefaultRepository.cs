using System;
using FormsApi.Repository.Query;

namespace FormsApi.Repository;

internal sealed class DefaultRepository<T> : IRepository<T>
    where T : new()
{
    public async Task DeleteAsync(T toDelete) => throw new NotSupportedException();
    public async Task<IEnumerable<T>> GetAsync(QueryCriteria criteria) => throw new NotSupportedException();
    public async Task<T> GetNewAsync() => new();
    public Task SaveAsync(T toSave) => throw new NotSupportedException();
}
