using FormsApi.Repository.Query;

namespace FormsApi.Repository.Service;

internal interface IReadableRepositoryService
{
    Task<object> GetNewAsync();
    Task<IEnumerable<object>> GetAsync(QueryCriteria criteria);
}

internal sealed class ReadableRepositoryService<T>(
    IRepositoryHandler<T> handler) : IReadableRepositoryService
{
    public async Task<IEnumerable<object>> GetAsync(QueryCriteria criteria)
    {
        return (IEnumerable<object>)handler.GetAsync(criteria);
    }
    public async Task<object> GetNewAsync()
    {
        return handler.GetNewAsync();
    }
}
