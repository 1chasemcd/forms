namespace FormsApi.Repository.Handler;

public interface IRepositorySaveHandler<T>
{
    Task SaveAsync(T toSave);
}
