using FormsApi.Repository.Query;

namespace FormsApi.Repository.Service;

internal interface IReadableRepositoryService
{
    Task<object> GetNewAsync();
    Task<IEnumerable<object>> GetAsync(QueryCriteria criteria);
}

internal sealed class ReadableRepositoryService<T>(
    IRepository<T> repository) : IReadableRepositoryService
{
    public async Task<IEnumerable<object>> GetAsync(QueryCriteria criteria)
    {
        return (IEnumerable<object>)repository.GetAsync(criteria);
    }
    public async Task<object> GetNewAsync()
    {
        return repository.GetNewAsync();
    }
}
