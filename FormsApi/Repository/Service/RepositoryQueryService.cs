using FormsApi.Repository.Handler;

namespace FormsApi.Repository.Service;

public sealed class RepositoryQueryService<T>(IRepositoryQueryHandler<T> repository, string? id = null) : IRepositoryCallable
    where T : class
{
    public async Task<object?> Invoke()
    {
        if (id == null)
            return await repository.GetAllAsync();
        else
            return await repository.GetAsync(id);
    }
}
