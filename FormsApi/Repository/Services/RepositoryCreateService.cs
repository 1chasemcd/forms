using FormsApi.Repository.Handlers;

namespace FormsApi.Repository.Services;

public sealed class RepositoryCreateService<T>(IRepositoryCreateHandler<T> repository) : IRepositoryCallable
{

    public async Task<object?> Invoke()
    {
        return await repository.CreateAsync();
    }
}
