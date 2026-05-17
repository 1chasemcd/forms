namespace FormsApi.Repository.Handlers;

public interface IRepositoryDeleteHandler<T>
{
    Task DeleteAsync(T toDelete);
}
