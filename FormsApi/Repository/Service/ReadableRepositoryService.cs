using FormsApi.Repository.Query;

namespace FormsApi.Repository.Service;

public interface IReadableRepositoryService
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
        return (await Repository.GetAsync(criteria)).Cast<object>();
    }
    public async Task<object> GetNewAsync()
    {
        return (await Repository.GetNewAsync()) as object ?? string.Empty;
    }
}
