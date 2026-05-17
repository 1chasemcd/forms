namespace FormsApi.Repository.Handlers;

public interface IRepositoryQueryHandler<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetAsync(string id);
}
