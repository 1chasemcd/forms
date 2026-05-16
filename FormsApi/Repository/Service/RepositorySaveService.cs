using FormsApi.Repository.Handler;

namespace FormsApi.Repository.Service;

public sealed class RepositorySaveService<T>(IRepositorySaveHandler<T> repository, T model) : IRepositoryCallable
{
    public async Task<object?> Invoke()
    {
        await repository.SaveAsync(model);
        return new object();
    }
}
