using FormsApi.Repository.Handler;
using Microsoft.AspNetCore.Http;

namespace FormsApi.Repository.Service;

public class RepositoryQueryService<T>(IRepositoryQueryHandler<T> repository, string? id = null) : IRepositoryCallable
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
