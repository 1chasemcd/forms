using FormsApi.Repository.Query;

namespace FormsApi.Repository;

public interface IRepositoryHandler<T>
{
    Task<T> GetNewAsync();
    Task<IEnumerable<T>> GetAsync(QueryCriteria criteria);
    Task SaveAsync(T toSave);
    Task DeleteAsync(T toDelete);
}
