using FormsApi.Repository.Handlers;

namespace FormsApi.Repository.Services;

public sealed class RepositorySaveService<T>(IRepositorySaveHandler<T> repository, T model) : IRepositoryCallable
{
    public async Task<object?> Invoke()
    {
        await repository.SaveAsync(model);
        return new object();
    }
}
