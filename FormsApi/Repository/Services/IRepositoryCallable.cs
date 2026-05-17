namespace FormsApi.Repository.Services;

public interface IRepositoryCallable
{
    Task<object?> Invoke();
}
