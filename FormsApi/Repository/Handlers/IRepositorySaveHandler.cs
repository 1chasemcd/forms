namespace FormsApi.Repository.Handlers;

public interface IRepositorySaveHandler<T>
{
    Task SaveAsync(T toSave);
}
