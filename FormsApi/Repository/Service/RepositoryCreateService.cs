using FormsApi.Repository.Handler;

namespace FormsApi.Repository.Service;

public sealed class RepositoryCreateService<T>(IRepositoryCreateHandler<T> repository) : IRepositoryCallable
{

    public async Task<object?> Invoke()
    {
        return await repository.CreateAsync();
    }
}
