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
    internal IRepository<T> Repository { get; init; } = repository;
    public async Task<IEnumerable<object>> GetAsync(QueryCriteria criteria)
    {
        return (IEnumerable<object>)Repository.GetAsync(criteria);
    }
    public async Task<object> GetNewAsync()
    {
        return Repository.GetNewAsync();
    }
}
