namespace FormsApi.Repository.Handlers;

public interface IRepositoryCreateHandler<T>
{
    Task<T> CreateAsync();
}
